using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace CPM_WIN
{
    public partial class Form1 : Form
    {
        #region Constants
        private const int UI_TIMER_INTERVAL_MS = 200;
        private const int MAX_PLAYER_SELECTION_ATTEMPTS = 100;
        private const int DEFAULT_SPEECH_RATE = 3;
        private const int DEFAULT_SPEECH_VOLUME = 100;
        private const int MAX_LEVEL = 7;
        private const int MIN_LEVEL = 1;
        private const int AUTO_START_DELAY_MS = 60000; // 1 minute in milliseconds
        private const int AUTO_START_DELAY_MINUTES = AUTO_START_DELAY_MS / 60000;
        private const string FILE_LAST_GAME = "Last Game.txt";
        private const string FILE_FULL_LOG = "Full Log.txt";
        private const string FILE_PLAYER_REPORT = "Player Report.txt";
        private const string FILE_LEVEL_PROGRESS = "Level Progress.txt";
        private const string FILE_PLAYER_NAME = "Player Name.txt";
        #endregion

        #region Instance Variables
        private int CurrentLevel = MIN_LEVEL;
        private readonly Timer _uiTimer;
        private readonly Timer _autoStartTimer;
        private readonly SpeechSynthesizer _synth;
        private readonly object _synthLock = new object();
        private readonly Stopwatch _gameTimer = new Stopwatch();
        private readonly Random _rng = new Random();
        private bool[] _chosen = new bool[8];
        private TimeSpan _nextSpeakTime = TimeSpan.FromMinutes(10);
        private TimeSpan _speakInterval = TimeSpan.FromMinutes(10);
        private bool _newGameStarted = false;
        public static string HumanPlayerName;
        #endregion

        #region Static Variables
        private static readonly string[] PlayerNames = 
        { 
            "Arthur", "Gertrude", "Erwin", "Maude", "Carmen", "Isaac", "Penelope", "Ollie" 
        };
        public static string AppDirectory { get; private set; }
        public static string LastGamePath { get; private set; }
        public static string FullLogPath { get; private set; }
        public static string PlayerReportPath { get; private set; }
        public static string PlayerNamePath { get; private set; } = string.Empty;
        
        // Temporary bridge for ReportViewer - consider refactoring to dependency injection
        private static Form1 _instance;
        #endregion

        static Form1()
        {
            AppDirectory = InitializeAppDirectory();
            LastGamePath = Path.Combine(AppDirectory, FILE_LAST_GAME);
            FullLogPath = Path.Combine(AppDirectory, FILE_FULL_LOG);
            PlayerReportPath = Path.Combine(AppDirectory, FILE_PLAYER_REPORT);
        }

        private static string InitializeAppDirectory()
        {
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

            string appDir = Path.Combine(documentsRoot, "CPM");

            try
            {
                if (!Directory.Exists(appDir))
                    Directory.CreateDirectory(appDir);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to ensure AppDirectory: {ex.Message}");
            }

            return appDir;
        }

        public Form1()
        {
            InitializeComponent();
            _instance = this;

            _synth = new SpeechSynthesizer();
            try
            {
                _synth.Rate = DEFAULT_SPEECH_RATE;
                _synth.Volume = DEFAULT_SPEECH_VOLUME;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Speech initialization failed: {ex.Message}");
            }

            _uiTimer = new Timer { Interval = UI_TIMER_INTERVAL_MS };
            _uiTimer.Tick += UiTimer_Tick;
            _uiTimer.Start();

            _autoStartTimer = new Timer { Interval = AUTO_START_DELAY_MS };
            _autoStartTimer.Tick += AutoStartTimer_Tick;

            timerToolStripMenuItem.Enabled = false;
            LoadPlayerName();
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            var elapsed = _gameTimer.Elapsed;
            GameTimeTB.Text = elapsed.TotalHours >= 1 
                ? elapsed.ToString(@"h\:mm\:ss") 
                : elapsed.ToString(@"mm\:ss");

            if (_gameTimer.IsRunning)
            {
                TryScheduleSpeak(elapsed);
            }
        }

        private void AutoStartTimer_Tick(object sender, EventArgs e)
        {
            _autoStartTimer.Stop();

            if (!_gameTimer.IsRunning)
            {
                _gameTimer.Start();
                SpeakAsync("Game timer has started.");
            }
        }

        private void TryScheduleSpeak(TimeSpan elapsed)
        {
            if (elapsed < _nextSpeakTime)
                return;

            int intervalMinutes = Math.Max(1, (int)_speakInterval.TotalMinutes);
            int multiplier = (int)elapsed.TotalMinutes / intervalMinutes;
            var computedNext = TimeSpan.FromMinutes((multiplier + 1) * intervalMinutes);

            lock (_synthLock)
            {
                try
                {
                    if (_synth.State == SynthesizerState.Speaking)
                    {
                        _nextSpeakTime = computedNext;
                        return;
                    }

                    SpeakElapsedInternal(elapsed);
                    _nextSpeakTime = computedNext;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"TryScheduleSpeak failed: {ex.Message}");
                }
            }
        }

        private void SpeakElapsedInternal(TimeSpan ts)
        {
            int hours = (int)ts.TotalHours;
            int minutes = ts.Minutes;
            int seconds = ts.Seconds;

            string phrase;
            if (hours > 0)
            {
                phrase = $"Elapsed time: {hours} hour{Pluralize(hours)} {minutes} minute{Pluralize(minutes)}.";
            }
            else if (minutes > 0)
            {
                phrase = $"Elapsed time: {minutes} minute{Pluralize(minutes)} {seconds} second{Pluralize(seconds)}.";
            }
            else
            {
                phrase = $"Elapsed time: {seconds} second{Pluralize(seconds)}.";
            }

            try
            {
                _synth?.SpeakAsync(phrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SpeakElapsedInternal failed: {ex.Message}");
            }
        }

        private static string Pluralize(int count)
        {
            return count != 1 ? "s" : "";
        }

        private void SpeakAsync(string text)
        {
            lock (_synthLock)
            {
                try
                {
                    _synth?.SpeakAsync(text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SpeakAsync failed: {ex.Message}");
                }
            }
        }

        private void Speak(string text)
        {
            lock (_synthLock)
            {
                try
                {
                    _synth?.Speak(text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Speak failed: {ex.Message}");
                }
            }
        }

        // Public method for external speech requests (e.g., from ReportViewer)
        public static void SpeakText(string text)
        {
            _instance?.SpeakAsync(text);
        }

        public static void CancelAllSpeech()
        {
            if (_instance?._synth != null)
            {
                lock (_instance._synthLock)
                {
                    try
                    {
                        _instance._synth.SpeakAsyncCancelAll();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"CancelAllSpeech failed: {ex.Message}");
                    }
                }
            }
        }

        public void SaveLastGame()
        {
            try
            {
                EnsureDirectoryExists(AppDirectory);

                using (var writer = new StreamWriter(LastGamePath, false))
                {
                    writer.WriteLine($"          Date & Time:  {DateTimeTB.Text}");
                    writer.WriteLine($"# of Players:  {NumPlayersTB.Text}");
                    writer.WriteLine($"         Player Names:  {NamesTB.Text}");
                    writer.WriteLine($"            Game Rule:  {GameRuleTB.Text}");
                    writer.WriteLine($"         Elapsed Time:  {GameTimeTB.Text}");
                    writer.WriteLine($"         Total Assets:  {AssetsNUD.Value.ToString("C", CultureInfo.CurrentCulture)}");
                    writer.WriteLine($"               Result:  {(AssetsNUD.Value > 0 ? "Win" : "Loss")}");
                    writer.WriteLine("\n--------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving game: {ex.Message}", "Save Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveLevel()
        {
            string levelFile = Path.Combine(AppDirectory, FILE_LEVEL_PROGRESS);

            int nextLevel = CurrentLevel + 1;

            try
            {
                if (nextLevel > MAX_LEVEL || AssetsNUD.Value <= 0)
                {
                    DeleteFileIfExists(levelFile);
                    return;
                }

                if (AssetsNUD.Value > 0)
                {
                    File.WriteAllText(levelFile, nextLevel.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveLevel failed: {ex.Message}");
            }
        }

        public int LoadLevel()
        {
            string levelFile = Path.Combine(AppDirectory, FILE_LEVEL_PROGRESS);

            try
            {
                if (File.Exists(levelFile))
                {
                    string content = File.ReadAllText(levelFile);
                    if (int.TryParse(content, out int savedLevel) && 
                        savedLevel >= MIN_LEVEL && savedLevel <= MAX_LEVEL)
                    {
                        CurrentLevel = savedLevel;
                        return savedLevel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadLevel failed: {ex.Message}");
            }
            return MIN_LEVEL;
        }

        public void NewGame()
        {
            if (HumanPlayerName.Length < 3)
            {
                MessageBox.Show("Please type in your name by clicking Edit Human Name in the menu.  Name must be at least 3 characters.\n\nYour name will be saved for future games.",  
                    "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        
        Array.Clear(_chosen, 0, _chosen.Length);
            NamesTB.Text = string.Empty;

            DateTimeTB.Text = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            NumPlayersTB.Text = $"{CurrentLevel} Computer, {CurrentLevel + 1} Total";

            int ComputerPlayersToPick = CurrentLevel;
            int TotalPlayers = ComputerPlayersToPick + 1; // Computer players + 1 human player
            int HumanPlayerIndex = _rng.Next(0, TotalPlayers);

            for (int i = 0; i < TotalPlayers; i++)
            {
                if (i == HumanPlayerIndex)
                {
                    NamesTB.Text += HumanPlayerName + " - ";
                }
                else
                {
                    GetPlayer();
                }
            }

            NamesTB.Text = NamesTB.Text.TrimEnd(' ', '-');
            DeleteFileIfExists(LastGamePath);

            timerToolStripMenuItem.Enabled = true;
            GameRule();
            _gameTimer.Reset();
            _nextSpeakTime = _speakInterval;
            _newGameStarted = true;

            ReadGameInfo();
            SpeakAsync($"New game started, Game timer will automatically start in {AUTO_START_DELAY_MINUTES} {WordChoice("minute", "minutes", AUTO_START_DELAY_MINUTES)}.");

            _autoStartTimer.Start();
        }

        private string WordChoice(string singular, string plural, int value)
        {
            if (value == 1)
            {
                return singular;
            }
            else
            {
                return plural;
            }
        }

        public void SaveFullLog()
        {
            try
            {
                if (!File.Exists(LastGamePath))
                    return;

                string lastRunContents = File.ReadAllText(LastGamePath);
                EnsureDirectoryExists(Path.GetDirectoryName(FullLogPath));
                File.AppendAllText(FullLogPath, lastRunContents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveFullLog failed: {ex.Message}");
            }
        }

        public void GeneratePlayerReport()
        {
            try
            {
                if (!File.Exists(FullLogPath))
                    return;

                long wins = 0, losses = 0, currentStreak = 0, longestStreak = 0;

                string[] lines = File.ReadAllLines(FullLogPath);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (line.Contains("Result:") && line.Contains("Win"))
                    {
                        wins++;
                        currentStreak++;
                        longestStreak = Math.Max(longestStreak, currentStreak);
                    }
                    else if (line.Contains("Result:") && line.Contains("Loss"))
                    {
                        losses++;
                        currentStreak = 0;
                    }
                }

                using (var writer = new StreamWriter(PlayerReportPath, false))
                {
                    writer.WriteLine($"NES CPM Player Report for {HumanPlayerName}");
                    writer.WriteLine(DateTime.Now.ToLongDateString());
                    writer.WriteLine();
                    writer.WriteLine();
                    writer.WriteLine($"      Games Played:  {wins + losses}");
                    writer.WriteLine($"              Wins:  {wins}");
                    writer.WriteLine($"Longest Win Streak:  {longestStreak}");

                    double totalGames = wins + losses;
                    double winRate = totalGames > 0 ? (wins / totalGames) * 100.0 : 0.0;
                    writer.WriteLine($"          Win Rate:  {winRate:F2}%");
                    writer.WriteLine($"            Losses:  {losses}");
                    writer.WriteLine($"     Current Level:  {LoadLevel()}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GeneratePlayerReport failed: {ex.Message}");
            }
        }

        public void GameRule()
        {
            string[] laws =
            {
                "No trades until computer offers a trade first.",
                "For your first monopoly, you must build from scratch to hotels in one turn.",
                "Always roll to get out of jail.",
                "Always pay to get out of jail.",
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

            int lawNum = _rng.Next(0, laws.Length);
            GameRuleTB.Text = laws[lawNum];
        }

        public void GetPlayer()
        {
            if (_chosen.All(c => c))
            {
                Array.Clear(_chosen, 0, _chosen.Length);
            }

            for (int attempts = 0; attempts < MAX_PLAYER_SELECTION_ATTEMPTS; attempts++)
            {
                int playerNum = _rng.Next(0, PlayerNames.Length);

                if (!_chosen[playerNum])
                {
                    _chosen[playerNum] = true;
                    NamesTB.Text += $"{PlayerNames[playerNum]} - ";
                    return;
                }
            }

            // Fallback: pick first available
            for (int i = 0; i < PlayerNames.Length; i++)
            {
                if (!_chosen[i])
                {
                    _chosen[i] = true;
                    NamesTB.Text += $"{PlayerNames[i]} - ";
                    return;
                }
            }
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void DeleteFileIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Delete file failed: {ex.Message}");
                }
            }
        }

        private void SavePlayerName()
        {
            try
            {
                EnsureDirectoryExists(AppDirectory);
                using (var writer = new StreamWriter(Path.Combine(AppDirectory, FILE_PLAYER_NAME), false))
                {
                    writer.WriteLine(HumanPlayerName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SavePlayerName failed: {ex.Message}");
            }
        }
        
        private void LoadPlayerName()
        {
            string playerNamePath = Path.Combine(AppDirectory, FILE_PLAYER_NAME);

            try
            {
                if (!File.Exists(playerNamePath))
                {
                    HumanPlayerName = "";
                    return;
                }

                using (var reader = new StreamReader(playerNamePath))
                {
                    HumanPlayerName = reader.ReadLine() ?? "";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadPlayerName failed: {ex.Message}");
                HumanPlayerName = "";
            }
        }

        private void form1_shown(object sender, EventArgs e)
        {
            AssetsNUD.Focus();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            try
            {
                _uiTimer?.Stop();
                _autoStartTimer?.Stop();
                SavePlayerName();

                lock (_synthLock)
                {
                    try
                    {
                        _synth?.SpeakAsyncCancelAll();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to cancel speech during closing: {ex.Message}");
                    }
                }

                if (_newGameStarted)
                {
                    SaveLastGame();
                    SaveFullLog();
                    GeneratePlayerReport();
                    SaveLevel();
                }

                _uiTimer?.Dispose();
                _autoStartTimer?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"OnFormClosing cleanup failed: {ex.Message}");
            }

            try
            {
                lock (_synthLock)
                {
                    _synth?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed disposing synth: {ex.Message}");
            }

            _instance = null;
        }

        #region Menu Event Handlers
        private void saveExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void readToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadGameInfo();
        }

        private void ReadGameInfo()
        {
            if (!_newGameStarted)
                return;

            Speak("Reading game information.");
            Speak($"Date & Time:  {DateTimeTB.Text}");
            Speak($"Number of Players:  {NumPlayersTB.Text}");
            Speak($"Player names:  {NamesTB.Text}");
            Speak($"Game rule:  {GameRuleTB.Text}");
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gameTimer.Reset();
            _nextSpeakTime = _speakInterval;
            SpeakAsync("Game timer has been reset.");
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_gameTimer.IsRunning)
            {
                SpeakAsync("Game timer is already running.");
            }
            else
            {
                _gameTimer.Start();
                SpeakAsync("Game timer started.");
            }
        }

        private void stoPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_gameTimer.IsRunning)
            {
                _gameTimer.Stop();
                SpeakAsync("Game timer stopped.");
            }
            else
            {
                SpeakAsync("Game timer is already stopped.");
            }
        }

        private void viewRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneratePlayerReport();
            using (var prForm = new ReportViewer())
            {
                prForm.ShowDialog();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_newGameStarted)
            {
                SpeakAsync("A game is already in progress.");
                return;
            }

            NewGame();
        }

        private void speakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpeakTimer();
        }

        private void SpeakTimer()
        {
            string status = _gameTimer.IsRunning ? "running" : "stopped";
            Speak($"The game timer is currently {status}, and the elapsed time is {_gameTimer.Elapsed:hh\\:mm\\:ss}.");
        }
        #endregion

        private void editHumanNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var playerNameForm = new PlayerName())
            {
                playerNameForm.ShowDialog();
            }
        }
    }
}