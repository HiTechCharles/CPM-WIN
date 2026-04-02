using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Speech.Synthesis;
using System.Globalization;

namespace CPM_WIN
{
    public partial class Form1 : Form
    {
        #region Constants
        private const int UI_TIMER_INTERVAL_MS = 200;
        private const int MAX_SPEAK_CHARS = 4000;
        private const int MAX_PLAYER_SELECTION_ATTEMPTS = 100;
        private const int DEFAULT_SPEECH_RATE = 3;
        private const int DEFAULT_SPEECH_VOLUME = 100;
        private const int MAX_LEVEL = 7;
        private const int MIN_LEVEL = 1;
        #endregion

        #region Static Variables
        public static Stopwatch gameTimer = new Stopwatch();
        public static Random RNG = new Random();
        public static bool[] Chosen = new bool[8];  // has a player been chosen yet?
        public static string[] PlayerName = { "Arthur", "Gertrude", "Erwin", "Maude", "Carmen", "Isaac", "Penelope", "Ollie" };  // player names
        public static string AppDirectory;
        public static string LastGame;
        public static string FullLog;
        public static string PlayerReport;
        public static SpeechSynthesizer synth = new SpeechSynthesizer();
        public static object synthLock = new object();
        public static object rngLock = new object();
        public static bool NewGameStarted = false;  // flag to indicate if a new game has started
        public static TimeSpan nextSpeakTime = TimeSpan.FromMinutes(10);  // next time to speak elapsed
        public static TimeSpan speakInterval = TimeSpan.FromMinutes(10);  // interval between speaks
        public static Timer uiTimer;  // UI timer to update GameTimeTB periodically
        #endregion

        static Form1()  // Static constructor to initialize paths robustly
        {
            // Prefer OneDrive consumer Documents folder when available, otherwise fall back to the user's Documents folder
            var oneDriveDocs = Environment.GetEnvironmentVariable("onedriveconsumer");
            string documentsRoot;

            if (!string.IsNullOrWhiteSpace(oneDriveDocs))
            {
                try
                {
                    var oneDriveDocsPath = Path.Combine(oneDriveDocs, "documents");
                    documentsRoot = Directory.Exists(oneDriveDocsPath)
                        ? oneDriveDocsPath
                        : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                catch
                {
                    documentsRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
            }
            else
            {
                documentsRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            AppDirectory = Path.Combine(documentsRoot, "CPM");
            LastGame = Path.Combine(AppDirectory, "Last Game.txt");
            FullLog = Path.Combine(AppDirectory, "Full Log.txt");
            PlayerReport = Path.Combine(AppDirectory, "Player Report.txt");

            // Ensure directory exists
            try
            {
                if (!Directory.Exists(AppDirectory))
                    Directory.CreateDirectory(AppDirectory);
            }
            catch (Exception ex)
            {
                // Avoid throwing from static ctor; log instead
                Debug.WriteLine("Failed to ensure AppDirectory: " + ex.Message);
            }
        }

        public Form1()  // Constructor
        {
            InitializeComponent();
            try
            {
                synth.Rate = DEFAULT_SPEECH_RATE;  // set speech rate
                synth.Volume = DEFAULT_SPEECH_VOLUME;  // set speech volume
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Speech initialization failed: " + ex.Message);
            }

            // Update the GameTimeTB every 200 ms
            uiTimer = new Timer { Interval = UI_TIMER_INTERVAL_MS };
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();

            timerToolStripMenuItem.Enabled = false;  // Disable timer menu
        }

        public void UiTimer_Tick(object sender, EventArgs e)
        {
            // Update the game timer display
            var elapsed = gameTimer.Elapsed;

            // Format and display elapsed time
            if (elapsed.TotalHours >= 1)
            {
                GameTimeTB.Text = elapsed.ToString(@"h\:mm\:ss");
            }
            else
            {
                GameTimeTB.Text = elapsed.ToString(@"mm\:ss");
            }

            if (!gameTimer.IsRunning)
                return;

            TryScheduleSpeak(elapsed);
        }

        private void TryScheduleSpeak(TimeSpan elapsed)
        {
            // Fast check to avoid taking the synth lock unnecessarily
            if (elapsed < nextSpeakTime)
                return;

            // Compute the next speak time using integer minutes to avoid rounding surprises
            int intervalMinutes = (int)speakInterval.TotalMinutes;
            if (intervalMinutes <= 0)
                intervalMinutes = 1; // defensive

            int multiplier = (int)elapsed.TotalMinutes / intervalMinutes;
            var computedNext = TimeSpan.FromMinutes((multiplier + 1) * intervalMinutes);

            // Lock synth to avoid queueing overlapping speech or racing with Dispose/SpeakAsyncCancelAll
            lock (synthLock)
            {
                try
                {
                    // If the synthesizer is already speaking, skip scheduling another phrase.
                    // This prevents long queues of overlapping "elapsed time" announcements.
                    if (synth != null && synth.State == SynthesizerState.Speaking)
                    {
                        // update nextSpeakTime so we don't repeatedly try again until the next interval
                        nextSpeakTime = computedNext;
                        return;
                    }

                    // Speak without double-locking: call the internal method that does NOT re-lock.
                    SpeakElapsedInternal(elapsed);

                    // Update nextSpeakTime to the next interval target
                    nextSpeakTime = computedNext;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("TryScheduleSpeak failed: " + ex.Message);
                }
            }
        }

        // Public API preserved for callers that expect SpeakElapsed to lock internally.
        // This keeps existing callers working while allowing TryScheduleSpeak to call the internal worker.
        public void SpeakElapsed(TimeSpan ts)
        {
            lock (synthLock)
            {
                SpeakElapsedInternal(ts);
            }
        }

        private void SpeakElapsedInternal(TimeSpan ts)
        {
            // Build a concise, human-friendly phrase (no locks here)
            int hours = (int)ts.TotalHours;
            int minutes = ts.Minutes;
            int seconds = ts.Seconds;

            string phrase;
            if (hours > 0)
            {
                phrase = $"Elapsed time: {hours} hour{(hours > 1 ? "s" : "")} {minutes} minute{(minutes != 1 ? "s" : "")}.";
            }
            else if (minutes > 0)
            {
                phrase = $"Elapsed time: {minutes} minute{(minutes != 1 ? "s" : "")} {seconds} second{(seconds != 1 ? "s" : "")}.";
            }
            else
            {
                phrase = $"Elapsed time: {seconds} second{(seconds != 1 ? "s" : "")}.";
            }

            try
            {
                // synth is expected to be locked by callers that need to avoid races with Dispose.
                // Call SpeakAsync directly; it queues the speech on a background thread.
                synth?.SpeakAsync(phrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SpeakElapsedInternal failed: " + ex.Message);
            }
        }

        public void SaveLastGame()  // save game function
        {
            try
            {
                // Ensure app directory exists
                if (!Directory.Exists(AppDirectory))
                    Directory.CreateDirectory(AppDirectory);

                using (StreamWriter Writer = new StreamWriter(LastGame, false))
                {
                    Writer.WriteLine("          Date & Time:  " + DateTimeTB.Text);  // write date and time
                    Writer.WriteLine("# of Computer Players:  " + NUMCPUTB.Text);  // write number of computer players
                    Writer.WriteLine("         Player Names:  " + NamesTB.Text);  // write computer player names
                    Writer.WriteLine("            Game Rule:  " + GameRuleTB.Text);  // write game rule
                    Writer.WriteLine("         Elapsed Time:  " + GameTimeTB.Text);  // write game time
                    Writer.WriteLine("         Total Assets:  " + AssetsNUD.Value.ToString("C", CultureInfo.CurrentCulture));  // write total assets

                    if (AssetsNUD.Value > 0)
                    {
                        Writer.WriteLine("               Result:  Win");
                    }
                    else
                    {
                        Writer.WriteLine("               Result:  Loss");
                    }
                    Writer.WriteLine("\n--------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving game: " + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveLevel()  // save level function
        {
            string LevelFile = Path.Combine(AppDirectory, "Level Progress.txt");
            if (!int.TryParse(NUMCPUTB.Text, out int currentLevel))
            {
                currentLevel = MIN_LEVEL;
            }
            int nextLevel = currentLevel + 1;

            try
            {
                if (nextLevel > MAX_LEVEL || AssetsNUD.Value <= 0)  // if max level reached or no assets, do not save
                {
                    if (File.Exists(LevelFile))
                    {
                        try { File.Delete(LevelFile); }
                        catch (Exception exDel) { Debug.WriteLine("Delete LevelFile failed: " + exDel.Message); }
                    }
                    return;
                }

                if (AssetsNUD.Value > 0)
                {
                    // Save level progress
                    using (StreamWriter writer = new StreamWriter(LevelFile, false))
                    {
                        writer.WriteLine(nextLevel.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveLevel failed: " + ex.Message);
            }
        }

        public int LoadLevel()  // load level function
        {
            string LevelFile = Path.Combine(AppDirectory, "Level Progress.txt");
            try
            {
                if (File.Exists(LevelFile))
                {
                    using (StreamReader reader = new StreamReader(LevelFile))
                    {
                        string line = reader.ReadLine();
                        if (int.TryParse(line, out int savedLevel))
                        {
                            if (savedLevel >= MIN_LEVEL && savedLevel <= MAX_LEVEL)
                            {
                                NUMCPUTB.Text = savedLevel.ToString();
                                return savedLevel;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadLevel failed: " + ex.Message);
            }

            NUMCPUTB.Text = MIN_LEVEL.ToString();  // default to level 1 if no file or error
            return MIN_LEVEL;  // default return value
        }

        public void NewGame()
        {
            // Reset chosen players array
            Array.Clear(Chosen, 0, Chosen.Length);

            // Clear previous player names
            NamesTB.Text = string.Empty;

            int savedLevel = LoadLevel();  // load saved level
            DateTimeTB.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

            int playersToPick = Math.Min(savedLevel, PlayerName.Length);
            for (int i = 0; i < playersToPick; i++)
            {
                GetPlayer();
            }

            // Remove any trailing separator
            NamesTB.Text = NamesTB.Text.TrimEnd(' ', '-');

            if (File.Exists(LastGame))
            {
                try { File.Delete(LastGame); }
                catch (Exception exDel) { Debug.WriteLine("Delete LastGame failed: " + exDel.Message); }
            }

            timerToolStripMenuItem.Enabled = true;  // Enable timer menu
            GameRule();
            gameTimer.Reset();

            // Reset next speak interval after new game
            nextSpeakTime = speakInterval;

            // Set flag to indicate a new game has started
            NewGameStarted = true;
        }

        public void SaveFullLog()  // save full log function
        {
            try
            {
                if (!File.Exists(LastGame))
                    return;

                string lastRunContents = File.ReadAllText(LastGame);

                // Ensure directory exists
                string dir = Path.GetDirectoryName(FullLog);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                    }
                    catch (Exception exDir)
                    {
                        Debug.WriteLine("SaveFullLog directory ensure failed: " + exDir.Message);
                        return;
                    }
                }

                File.AppendAllText(FullLog, lastRunContents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveFullLog failed: " + ex.Message);
            }
        }

        public void GeneratePlayerReport()
        {
            try
            {
                long wins = 0, losses = 0, currentStreak = 0, longestStreak = 0;

                // If no full log, return early
                if (!File.Exists(FullLog))
                {
                    return;
                }

                using (var reader = new StreamReader(FullLog))
                using (var writer = new StreamWriter(PlayerReport, false))
                {
                    writer.WriteLine("NES CPM Player Report");
                    writer.WriteLine(DateTime.Now.ToLongDateString());
                    writer.WriteLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // compute longest win streak and count wins/losses
                        if (line.Contains("Result:") && line.Contains("Win"))
                        {
                            wins++;
                            currentStreak++;
                            if (currentStreak > longestStreak)
                                longestStreak = currentStreak;
                        }
                        else if (line.Contains("Result:") && line.Contains("Loss"))
                        {
                            losses++;
                            currentStreak = 0;
                        }
                    }

                    double totalGames = (double)(wins + losses);
                    double winRate = totalGames > 0.0 ? (wins / totalGames) * 100.0 : 0.0;

                    writer.WriteLine();
                    writer.WriteLine("      Games Played:  " + (wins + losses).ToString());
                    writer.WriteLine("              Wins:  " + wins.ToString());
                    writer.WriteLine("Longest Win Streak:  " + longestStreak.ToString());
                    writer.WriteLine("          Win Rate:  " + winRate.ToString("F2") + "%");
                    writer.WriteLine("            Losses:  " + losses.ToString());
                    writer.WriteLine("     Current Level:  " + LoadLevel().ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GeneratePlayerReport failed: " + ex.Message);
            }
        }

        public void GameRule()  // things you should do during the game
        {
            string[] Law =
            {
                "No trades until computer offers a trade first.",
                "For your first monopoly, you must build from scratch to hotels in one turn.",
                "Always roll to get out of jail.",
                "Always pay to get out of jail",
                "Cannot buy any railroads.",
                "Cannot buy orange or red properties.",
                "Can only build on 1 monopoly.",
                "After getting a monopoly, you must pass go 5 times before building on it.",
                "Play one of the built-in scenarios from the game editor menu.  (1 to 4 players)",
                "All Players start with $500.",
                "All players start with $2500.",
                "Must keep at least $500 available at all times.",
                "You may use the rewind feature once during your game.",
                "Play a short game.",
                "Begin a normal game, and use a 60 minute timer.",
                "Auction off the first unowned property you land on."
            };

            int lawNum;
            lock (rngLock)
            {
                lawNum = RNG.Next(0, Law.Length);  // pick a number safely using shared RNG
            }

            GameRuleTB.Text = Law[lawNum];
        }

        public void GetPlayer() // pick players for the game
        {
            // If all players have been chosen, reset the flags so we don't infinite-loop
            if (Chosen.All(c => c))
            {
                Array.Clear(Chosen, 0, Chosen.Length);
            }

            int playerNum = 0;
            int attempts = 0;

            while (attempts < MAX_PLAYER_SELECTION_ATTEMPTS)
            {
                attempts++;
                lock (rngLock)
                {
                    playerNum = RNG.Next(0, PlayerName.Length);
                }

                if (!Chosen[playerNum])
                {
                    Chosen[playerNum] = true;
                    NamesTB.Text += PlayerName[playerNum] + " - ";
                    return;
                }
            }

            // Fallback: pick first available
            for (int i = 0; i < PlayerName.Length; i++)
            {
                if (!Chosen[i])
                {
                    Chosen[i] = true;
                    NamesTB.Text += PlayerName[i] + " - ";
                    return;
                }
            }
        }

        // Dispose managed resources that live beyond form lifetime
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            try
            {
                uiTimer?.Stop();

                // Try to cancel speech early to avoid blocking on dispose
                lock (synthLock)
                {
                    try
                    {
                        synth?.SpeakAsyncCancelAll();
                    }
                    catch (Exception exSpeakCancel)
                    {
                        Debug.WriteLine("Failed to cancel speech during closing: " + exSpeakCancel.Message);
                    }
                }

                if (NewGameStarted)  // only save if a new game was started
                {
                    SaveLastGame();  // save last game
                    SaveFullLog();  // save full log
                    GeneratePlayerReport();  // generate player report
                    SaveLevel();  // save level progress
                }

                uiTimer?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OnFormClosing cleanup failed: " + ex.Message);
            }

            try
            {
                lock (synthLock)
                {
                    synth?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed disposing synth: " + ex.Message);
            }
        }

        #region Menu Event Handlers
        private void saveExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void readToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Cancel quickly to avoid race with new speech
                lock (synthLock)
                {
                    synth?.SpeakAsyncCancelAll();
                }

                if (!File.Exists(LastGame))
                {
                    lock (synthLock)
                    {
                        synth?.SpeakAsync("No saved game found.");
                    }
                    return;
                }

                // Read file and limit spoken length to avoid overwhelming the synthesizer.
                // Do I/O outside of the synth lock to avoid blocking other speech operations.
                string todayLogContents = File.ReadAllText(LastGame);

                // Truncate if extremely long
                if (todayLogContents.Length > MAX_SPEAK_CHARS)
                    todayLogContents = todayLogContents.Substring(0, MAX_SPEAK_CHARS) + " ... (truncated)";

                lock (synthLock)
                {
                    synth?.SpeakAsync(todayLogContents);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("readToolStripMenuItem failed: " + ex.Message);
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameTimer.Reset();
            nextSpeakTime = speakInterval; // Reset speak timer as well

            try
            {
                lock (synthLock)
                {
                    synth?.SpeakAsync("Game timer has been reset.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Speech failed in reset: " + ex.Message);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gameTimer.IsRunning)
            {
                lock (synthLock)
                {
                    synth?.SpeakAsync("Game timer is already running.");
                }
            }
            else
            {
                gameTimer.Start();
                lock (synthLock)
                {
                    synth?.SpeakAsync("Game timer started.");
                }
            }
        }

        private void stoPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gameTimer.IsRunning)
            {
                gameTimer.Stop();
                lock (synthLock)
                {
                    synth?.SpeakAsync("Game timer stopped.");
                }
            }
            else
            {
                lock (synthLock)
                {
                    synth?.SpeakAsync("Game timer is already stopped.");
                }
            }
        }

        private void viewRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneratePlayerReport();
            ReportViewer prForm = new ReportViewer();
            prForm.ShowDialog();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewGameStarted)  // if a new game is already in progress
            {
                lock (synthLock)
                {
                    synth?.SpeakAsync("A game is already in progress.");
                }
                return;
            }

            NewGame();
        }
        #endregion
    }
}