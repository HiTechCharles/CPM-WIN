using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPM_WIN
{
    /// <summary>
    /// Main application form for the CPM (Classic Monopoly) game timer and manager.
    /// </summary>
    public partial class Form1 : Form
    {
        #region Constants
        private const int MIN_PLAYER_NAME_LENGTH = 3;
        private const string INVALID_NAME_MESSAGE = "Please type in your name by clicking Edit Human Name in the menu.  Name must be at least 3 characters.\n\nYour name will be saved for future games.";
        private const string INVALID_NAME_TITLE = "Invalid Name";
        private const string EMPTY_FIELD = "";
        private const string GAME_ALREADY_IN_PROGRESS = "A game is already in progress.";
        private const string NEW_GAME_MESSAGE_FORMAT = "New game started, Game timer will automatically start in {0} {1}.";
        private const string GAME_TIMER_ALREADY_RUNNING = "Game timer is already running.";
        private const string GAME_TIMER_STARTED = "Game timer started.";
        private const string GAME_TIMER_ALREADY_STOPPED = "Game timer is already stopped.";
        private const string GAME_TIMER_STOPPED = "Game timer stopped.";
        private const string GAME_TIMER_RESET = "Game timer has been reset.";
        private const string HUMAN_NAME_SET_FORMAT = "Human player name set to {0}";
        private const string EMULATOR_PATH_SET_FORMAT = "Emulator path set to {0}";
        private const string TEXT_TO_SPEECH_ENABLED = "Text to speech enabled.";
        private const string TEXT_TO_SPEECH_DISABLED = "Text to speech disabled.";
        private const string READING_GAME_INFORMATION = "Reading game information.";
        private const string DATE_TIME_FORMAT = "Date & Time:  {0}";
        private const string NUM_PLAYERS_FORMAT = "Number of Players:  {0}";
        private const string PLAYER_NAMES_FORMAT = "Player names:  {0}";
        private const string GAME_RULE_FORMAT = "Game rule:  {0}";
        private const string TIMER_STATUS_FORMAT = "The game timer is currently {0}, and the elapsed time is {1:hh\\:mm\\:ss}.";
        private const string TIMER_STATUS_RUNNING = "running";
        private const string TIMER_STATUS_STOPPED = "stopped";
        private const string SAVE_ERROR_TITLE = "Save Error";
        private const string EXECUTABLE_FILTER = "Executable Files (*.exe)|*.exe";
        #endregion

        #region Fields
        private readonly TimerManager _timerManager;
        private readonly LoggingManager _loggingManager;
        private readonly SpeechManager _speechManager;
        private readonly GameManager _gameManager;
        private readonly PlayerSelector _playerSelector;

        private static Form1 _instance;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the human player's name.
        /// </summary>
        public static string HumanPlayerName { get; private set; }

        // Static properties for backward compatibility with ReportViewer
        /// <summary>
        /// Gets the application directory path (for backward compatibility).
        /// </summary>
        public static string AppDirectory => _instance?._loggingManager.AppDirectory;

        /// <summary>
        /// Gets the last game file path (for backward compatibility).
        /// </summary>
        public static string LastGamePath => _instance?._loggingManager.LastGamePath;

        /// <summary>
        /// Gets the full log file path (for backward compatibility).
        /// </summary>
        public static string FullLogPath => _instance?._loggingManager.FullLogPath;

        /// <summary>
        /// Gets the player report file path (for backward compatibility).
        /// </summary>
        public static string PlayerReportPath => _instance?._loggingManager.PlayerReportPath;

        /// <summary>
        /// Gets the player name file path (for backward compatibility).
        /// </summary>
        public static string PlayerNamePath => _instance?._loggingManager.PlayerNamePath;
        #endregion

        /// <summary>
        /// Initializes a new instance of the Form1 class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            _instance = this;

            // Initialize managers (must be in constructor due to readonly fields)
            _loggingManager = new LoggingManager();
            _speechManager = new SpeechManager();
            _gameManager = new GameManager();
            _playerSelector = new PlayerSelector();
            _timerManager = new TimerManager();

            WireUpEventHandlers();
            InitializeMenuState();
            InitializeFocus();
        }

        /// <summary>
        /// Wires up event handlers for manager objects.
        /// </summary>
        private void WireUpEventHandlers()
        {
            _timerManager.OnTimerTick += TimerManager_OnTimerTick;
            _timerManager.OnSpeakScheduled += TimerManager_OnSpeakScheduled;
            _timerManager.OnAutoStartTick += TimerManager_OnAutoStartTick;
        }

        /// <summary>
        /// Handles the timer tick event to update the UI.
        /// </summary>
        private void TimerManager_OnTimerTick(object sender, TimeSpan elapsed)
        {
            GameTimeTB.Text = _timerManager.GetFormattedTime();
        }

        /// <summary>
        /// Handles the speak scheduled event to speak elapsed time.
        /// </summary>
        private void TimerManager_OnSpeakScheduled(object sender, TimeSpan elapsed)
        {
            if (_loggingManager.LoadTextToSpeech() && _speechManager.State != System.Speech.Synthesis.SynthesizerState.Speaking)
            {
                _speechManager.SpeakElapsedTime(elapsed);
            }
        }

        /// <summary>
        /// Handles the auto-start timer event.
        /// </summary>
        private void TimerManager_OnAutoStartTick(object sender, EventArgs e)
        {
            if (!_timerManager.IsRunning)
            {
                _timerManager.Start();
                MaybeSpeakAsync("Game timer has started.");
            }
        }

        /// <summary>
        /// Initializes the menu state.
        /// </summary>
        private void InitializeMenuState()
        {
            timerToolStripMenuItem.Enabled = false;
            HumanPlayerName = _loggingManager.LoadPlayerName();
            try
            {
                textToSpeechToolStripMenuItem.Checked = _loggingManager.LoadTextToSpeech();
            }
            catch
            {
                // Suppress initialization errors
            }
        }

        /// <summary>
        /// Runs the emulator if the path is set.
        /// </summary>
        private void RunEmulator()
        {
            try
            {
                string emulatorPath = _loggingManager.LoadEmulatorPath();
                if (!string.IsNullOrEmpty(emulatorPath))
                {
                    System.Diagnostics.Process.Start(emulatorPath);
                }
                else
                {
                    MessageBox.Show("Emulator path is not set. Please set it in the menu.", "Emulator Path Not Set", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start emulator: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Initializes the focus to the Assets input field.
        /// </summary>
        private void InitializeFocus()
        {
            this.Shown += (s, e) => AssetsNUD.Focus();
        }

        /// <summary>
        /// Starts a new game with the current settings.
        /// </summary>
        public async void NewGame()
        {
            if (!IsValidPlayerName())
            {
                MessageBox.Show(INVALID_NAME_MESSAGE, INVALID_NAME_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            InitializeGameUI();
            PrepareGameState();
            await ReadGameInfo();
            await AnnounceGameStart();
            _timerManager.StartAutoStartTimer();
            RunEmulator();
        }

        /// <summary>
        /// Checks if the player name is valid.
        /// </summary>
        /// <returns>True if the player name is valid; otherwise false.</returns>
        private bool IsValidPlayerName()
        {
            return !string.IsNullOrEmpty(HumanPlayerName) && HumanPlayerName.Length >= MIN_PLAYER_NAME_LENGTH;
        }

        /// <summary>
        /// Initializes the game UI with current game information.
        /// </summary>
        private void InitializeGameUI()
        {
            DateTimeTB.Text = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            NumPlayersTB.Text = _gameManager.GetNumPlayersText();
            NamesTB.Text = _playerSelector.GeneratePlayerList(_gameManager.CurrentLevel, HumanPlayerName);
            GameRuleTB.Text = _gameManager.GetRandomGameRule();
        }

        /// <summary>
        /// Prepares the game state for a new game.
        /// </summary>
        private void PrepareGameState()
        {
            _loggingManager.DeleteLastGame();
            timerToolStripMenuItem.Enabled = true;
            _timerManager.Reset();
            _gameManager.StartNewGame();
        }

        /// <summary>
        /// Announces the start of a new game.
        /// </summary>
        private Task AnnounceGameStart()
        {
            string minuteWord = WordChoice("minute", "minutes", TimerManager.AUTO_START_DELAY_MINUTES);
            string message = string.Format(NEW_GAME_MESSAGE_FORMAT, TimerManager.AUTO_START_DELAY_MINUTES, minuteWord);
            MaybeSpeak(message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns the singular or plural form of a word based on a count.
        /// </summary>
        /// <param name="singular">The singular form.</param>
        /// <param name="plural">The plural form.</param>
        /// <param name="value">The count value.</param>
        /// <returns>The singular form if value is 1; otherwise the plural form.</returns>
        private string WordChoice(string singular, string plural, int value)
        {
            return value == 1 ? singular : plural;
        }

        /// <summary>
        /// Saves the current game information to file.
        /// </summary>
        public void SaveLastGame()
        {
            try
            {
                _loggingManager.SaveLastGame(
                    DateTimeTB.Text,
                    NumPlayersTB.Text,
                    NamesTB.Text,
                    GameRuleTB.Text,
                    GameTimeTB.Text,
                    AssetsNUD.Value
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, SAVE_ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Speaks text if text-to-speech is enabled.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        public static void SpeakText(string text)
        {
            if (_instance != null && _instance._loggingManager.LoadTextToSpeech())
            {
                _instance._speechManager.SpeakAsync(text);
            }
        }

        /// <summary>
        /// Cancels all pending speech operations.
        /// </summary>
        public static void CancelAllSpeech()
        {
            _instance?._speechManager.CancelAll();
        }

        /// <summary>
        /// Reads and speaks the current game information.
        /// </summary>
        private Task ReadGameInfo()
        {
            if (!_gameManager.NewGameStarted)
                return Task.CompletedTask;

            MaybeSpeak(READING_GAME_INFORMATION);
            MaybeSpeak(string.Format(DATE_TIME_FORMAT, DateTimeTB.Text));
            MaybeSpeak(string.Format(NUM_PLAYERS_FORMAT, NumPlayersTB.Text));
            MaybeSpeak(string.Format(PLAYER_NAMES_FORMAT, NamesTB.Text));
            MaybeSpeak(string.Format(GAME_RULE_FORMAT, GameRuleTB.Text));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Speaks the current timer status.
        /// </summary>
        private void SpeakTimer()
        {
            string status = _timerManager.IsRunning ? TIMER_STATUS_RUNNING : TIMER_STATUS_STOPPED;
            string message = string.Format(TIMER_STATUS_FORMAT, status, _timerManager.Elapsed);
            MaybeSpeak(message);
        }

        /// <summary>
        /// Raises the FormClosing event and performs cleanup.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            PerformShutdown();
        }

        /// <summary>
        /// Performs cleanup and shutdown operations.
        /// </summary>
        private void PerformShutdown()
        {
            StopTimers();
            SavePlayerData();
            CancelSpeech();
            SaveGameData();
            DisposeManagers();
        }

        /// <summary>
        /// Stops all timers.
        /// </summary>
        private void StopTimers()
        {
            _timerManager?.StopAutoStartTimer();
        }

        /// <summary>
        /// Saves player-specific data.
        /// </summary>
        private void SavePlayerData()
        {
            if (!string.IsNullOrEmpty(HumanPlayerName))
            {
                _loggingManager?.SavePlayerName(HumanPlayerName);
            }
        }

        /// <summary>
        /// Cancels all speech operations.
        /// </summary>
        private void CancelSpeech()
        {
            _speechManager?.CancelAll();
        }

        /// <summary>
        /// Saves game data if a game was started.
        /// </summary>
        private void SaveGameData()
        {
            if (_gameManager.NewGameStarted)
            {
                SaveLastGame();
                _loggingManager.SaveFullLog();
                _loggingManager.GeneratePlayerReport(HumanPlayerName);
                _loggingManager.SaveLevel(_gameManager.CurrentLevel, AssetsNUD.Value);
            }
        }

        /// <summary>
        /// Disposes of all manager objects.
        /// </summary>
        private void DisposeManagers()
        {
            _timerManager?.Dispose();
            _speechManager?.Dispose();
            _instance = null;
        }

        #region Menu Event Handlers
        /// <summary>
        /// Handles the Save/Exit menu item click.
        /// </summary>
        private void saveExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the Read menu item click.
        /// </summary>
        private async void readToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_gameManager.NewGameStarted)
            {
                MaybeSpeakAsync("No game has been started yet.");
                return;
            }
            await ReadGameInfo();
        }

        /// <summary>
        /// Handles the Reset menu item click.
        /// </summary>
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _timerManager.Reset();
            MaybeSpeakAsync(GAME_TIMER_RESET);
        }

        /// <summary>
        /// Handles the Start menu item click.
        /// </summary>
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_timerManager.IsRunning)
            {
                MaybeSpeakAsync(GAME_TIMER_ALREADY_RUNNING);
            }
            else
            {
                _timerManager.Start();
                MaybeSpeakAsync(GAME_TIMER_STARTED);
            }
        }

        /// <summary>
        /// Handles the Stop menu item click.
        /// </summary>
        private void stoPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_timerManager.IsRunning)
            {
                _timerManager.Stop();
                MaybeSpeakAsync(GAME_TIMER_STOPPED);
            }
            else
            {
                MaybeSpeakAsync(GAME_TIMER_ALREADY_STOPPED);
            }
        }

        /// <summary>
        /// Handles the View Records menu item click.
        /// </summary>
        private void viewRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _loggingManager.GeneratePlayerReport(HumanPlayerName);
            using (var prForm = new ReportViewer())
            {
                prForm.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the New Game menu item click.
        /// </summary>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_gameManager.NewGameStarted)
            {
                _speechManager.SpeakAsync(GAME_ALREADY_IN_PROGRESS);
                return;
            }

            int level = _loggingManager.LoadLevel();
            _gameManager.SetLevel(level);
            NewGame();
        }

        /// <summary>
        /// Handles the Speak menu item click.
        /// </summary>
        private void speakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpeakTimer();
        }

        /// <summary>
        /// Handles the Form Shown event.
        /// </summary>
        private void form1_shown(object sender, EventArgs e)
        {
            AssetsNUD.Focus();
        }
        #endregion

        /// <summary>
        /// Sets the human player name and saves it.
        /// </summary>
        /// <param name="name">The player name to set.</param>
        public static void SetHumanPlayerName(string name)
        {
            HumanPlayerName = name;
            _instance?._loggingManager?.SavePlayerName(name);
        }

        /// <summary>
        /// Handles the Edit Human Name menu item click.
        /// </summary>
        private void humanNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var playerNameForm = new PlayerName())
            {
                playerNameForm.ShowDialog();
                // Reload the player name after editing
                HumanPlayerName = _loggingManager.LoadPlayerName();
                MaybeSpeakAsync(string.Format(HUMAN_NAME_SET_FORMAT, HumanPlayerName));
            }
        }

        /// <summary>
        /// Handles the Emulator Path menu item click.
        /// </summary>
        private void emulatorPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = EXECUTABLE_FILTER;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _loggingManager.SaveEmulatorPath(openFileDialog.FileName);
                    MaybeSpeakAsync(string.Format(EMULATOR_PATH_SET_FORMAT, openFileDialog.FileName));
                }
            }
        }

        /// <summary>
        /// Handles the Text-to-Speech menu item click.
        /// </summary>
        private void textToSpeechToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool enabled = !textToSpeechToolStripMenuItem.Checked;
            _loggingManager.SaveTextToSpeech(enabled);
            textToSpeechToolStripMenuItem.Checked = enabled;
            MaybeSpeak(enabled ? TEXT_TO_SPEECH_ENABLED : TEXT_TO_SPEECH_DISABLED);
        }

        /// <summary>
        /// Speaks text synchronously if text-to-speech is enabled.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        private void MaybeSpeak(string text)
        {
            if (_loggingManager.LoadTextToSpeech())
            {
                _speechManager.Speak(text);
            }
        }

        /// <summary>
        /// Speaks text asynchronously if text-to-speech is enabled.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        private void MaybeSpeakAsync(string text)
        {
            if (_loggingManager.LoadTextToSpeech())
            {
                _speechManager.SpeakAsync(text);
            }
        }
    }
}