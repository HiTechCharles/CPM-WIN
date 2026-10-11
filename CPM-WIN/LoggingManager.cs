using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPM_WIN
{
    /// <summary>
    /// Manages logging, reporting, and file operations for the CPM game application.
    /// </summary>
    public class LoggingManager
    {
        // Lock to protect file write/read operations for atomicity
        private readonly object _fileLock = new object();
        private const string FILE_LAST_GAME = "Last Game.txt";
        private const string FILE_FULL_LOG = "Full Log.txt";
        private const string FILE_PLAYER_REPORT = "Player Report.txt";
        private const string FILE_LEVEL_PROGRESS = "Level Progress.txt";
        private const string FILE_OPTIONS = "Options.xml";
        private const int MAX_LEVEL = 7;
        private const int MIN_LEVEL = 1;
        private const int MIN_WIN_STREAK_TRACKING = 1;
        private const string RESULT_LABEL = "Result:";
        private const string WIN_TEXT = "Win";
        private const string LOSS_TEXT = "Loss";

        /// <summary>
        /// Gets the application directory path.
        /// </summary>
        public string AppDirectory { get; private set; }

        /// <summary>
        /// Gets the path to the last game file.
        /// </summary>
        public string LastGamePath { get; private set; }

        /// <summary>
        /// Gets the path to the full log file.
        /// </summary>
        public string FullLogPath { get; private set; }

        /// <summary>
        /// Gets the path to the player report file.
        /// </summary>
        public string PlayerReportPath { get; private set; }

        public string CurrentSessionPath { get; private set; }   

        /// <summary>
        /// Gets the path to the player name options file.
        /// </summary>
        public string PlayerNamePath { get; private set; }

        private readonly OptionsManager _options;

        /// <summary>
        /// Initializes a new instance of the LoggingManager class.
        /// </summary>
        public LoggingManager()
        {
            AppDirectory = InitializeAppDirectory();
            CurrentSessionPath = Path.Combine(AppDirectory, "Current Session.xml");
            LastGamePath = Path.Combine(AppDirectory, FILE_LAST_GAME);
            FullLogPath = Path.Combine(AppDirectory, FILE_FULL_LOG);
            PlayerReportPath = Path.Combine(AppDirectory, FILE_PLAYER_REPORT);
            PlayerNamePath = Path.Combine(AppDirectory, FILE_OPTIONS);
            _options = new OptionsManager(AppDirectory);
        }

        /// <summary>
        /// Saves the current session state to disk (overwrites any existing session file).
        /// </summary>
        /// <param name="state">The session state to save.</param>
        public void SaveCurrentSession(SessionState state)
        {
            try
            {
                EnsureDirectoryExists(AppDirectory);
                // Serialize to a temp file then atomically replace the target on a background thread
                var serializer = new XmlSerializer(typeof(SessionState));
                AtomicWriteAsync(CurrentSessionPath, stream => serializer.Serialize(stream, state))
                    .ContinueWith(t => Debug.WriteLine($"SaveCurrentSession failed: {t.Exception?.GetBaseException().Message}"), TaskContinuationOptions.OnlyOnFaulted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveCurrentSession scheduling failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Async wrapper around AtomicWrite that performs the write on a background thread.
        /// </summary>
        /// <param name="targetPath">Path to write to.</param>
        /// <param name="writeAction">Action that writes to the provided stream.</param>
        /// <returns>A Task representing the background write operation.</returns>
        private Task AtomicWriteAsync(string targetPath, Action<Stream> writeAction)
        {
            return Task.Run(() => AtomicWrite(targetPath, writeAction));
        }

        /// <summary>
        /// Loads the saved current session state, or null if none exists or load fails.
        /// </summary>
        /// <returns>The loaded SessionState or null.</returns>
        public SessionState LoadCurrentSession()
        {
            try
            {
                if (!File.Exists(CurrentSessionPath))
                    return null;

                // Lock during read to avoid racing with writers
                lock (_fileLock)
                {
                    var serializer = new XmlSerializer(typeof(SessionState));
                    using (var stream = File.OpenRead(CurrentSessionPath))
                    {
                        var obj = serializer.Deserialize(stream) as SessionState;
                        return obj;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadCurrentSession failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Deletes the saved current session file if it exists.
        /// </summary>
        public void DeleteCurrentSession()
        {
            try
            {
                if (File.Exists(CurrentSessionPath))
                {
                    // Use lock to avoid deleting while another thread is writing
                    lock (_fileLock)
                    {
                        File.Delete(CurrentSessionPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteCurrentSession failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Initializes the application directory, creating it if necessary.
        /// </summary>
        /// <returns>The path to the application directory.</returns>
        private static string InitializeAppDirectory()
        {
            var oneDriveDocs = Environment.GetEnvironmentVariable("onedriveconsumer");
            string documentsRoot = DetermineDocumentsRoot(oneDriveDocs);
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

        /// <summary>
        /// Determines the documents root directory from OneDrive or system defaults.
        /// </summary>
        /// <param name="oneDrivePath">The OneDrive path from environment variables.</param>
        /// <returns>The documents root directory path.</returns>
        private static string DetermineDocumentsRoot(string oneDrivePath)
        {
            if (!string.IsNullOrWhiteSpace(oneDrivePath))
            {
                try
                {
                    var oneDriveDocsPath = Path.Combine(oneDrivePath, "documents");
                    if (Directory.Exists(oneDriveDocsPath))
                        return oneDriveDocsPath;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to use OneDrive path: {ex.Message}");
                }
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        /// <summary>
        /// Saves the last finished game's summary information to a file in the application directory.
        /// This writes a human-readable summary including date/time, players, rule, elapsed time and assets.
        /// </summary>
        /// <param name="dateTime">The date and time of the game (display string).</param>
        /// <param name="numPlayers">The human-readable number of players.</param>
        /// <param name="names">The player names string.</param>
        /// <param name="gameRule">The game rule text used in the game.</param>
        /// <param name="gameTime">The elapsed game time display string.</param>
        /// <param name="assets">The final asset total as a decimal.</param>
        /// <exception cref="Exception">Thrown when the save operation fails.
        /// Callers should present an error to the user when this occurs.</exception>
        public void SaveLastGame(string dateTime, string numPlayers, string names, string gameRule, string gameTime, decimal assets)
        {
            try
            {
                EnsureDirectoryExists(AppDirectory);
                // Perform last-game write in background to avoid blocking UI
                Task.Run(() =>
                {
                    try
                    {
                        AtomicWrite(LastGamePath, stream =>
                        {
                            using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, leaveOpen: true))
                            {
                                writer.WriteLine($"          Date & Time:  {dateTime}");
                                writer.WriteLine($"# of Players:  {numPlayers}");
                                writer.WriteLine($"         Player Names:  {names}");
                                writer.WriteLine($"            Game Rule:  {gameRule}");
                                writer.WriteLine($"         Elapsed Time:  {gameTime}");
                                writer.WriteLine($"         Total Assets:  {assets.ToString("C", CultureInfo.CurrentCulture)}");
                                writer.WriteLine($"               Result:  {(assets > 0 ? WIN_TEXT : LOSS_TEXT)}");
                                writer.WriteLine("\n--------------------------------------------------");
                                writer.Flush();
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SaveLastGame failed: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error scheduling save of last game: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves the last game to the full log file.
        /// </summary>
        public void SaveFullLog()
        {
            try
            {
                if (!File.Exists(LastGamePath))
                    return;
                string lastRunContents = File.ReadAllText(LastGamePath);
                EnsureDirectoryExists(Path.GetDirectoryName(FullLogPath));
                // Append on background thread under lock to avoid blocking UI and concurrent writers
                Task.Run(() =>
                {
                    try
                    {
                        lock (_fileLock)
                        {
                            using (var fs = new FileStream(FullLogPath, FileMode.Append, FileAccess.Write, FileShare.None))
                            {
                                var bytes = Encoding.UTF8.GetBytes(lastRunContents);
                                fs.Write(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SaveFullLog failed: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveFullLog failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the current level progress.
        /// </summary>
        /// <param name="level">The current level.</param>
        /// <param name="assets">The final assets total.</param>
        public void SaveLevel(int level, decimal assets)
        {
            string levelFile = Path.Combine(AppDirectory, FILE_LEVEL_PROGRESS);
            int nextLevel = level + 1;

            try
            {
                if (nextLevel > MAX_LEVEL || assets <= 0)
                {
                    DeleteFileIfExists(levelFile);
                    return;
                }

                if (assets > 0)
                {
                    EnsureDirectoryExists(Path.GetDirectoryName(levelFile));
                    // Write level progress asynchronously
                    AtomicWriteAsync(levelFile, stream =>
                    {
                        using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, leaveOpen: true))
                        {
                            writer.Write(nextLevel.ToString());
                            writer.Flush();
                        }
                    }).ContinueWith(t => Debug.WriteLine($"SaveLevel failed: {t.Exception?.GetBaseException().Message}"), TaskContinuationOptions.OnlyOnFaulted);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveLevel failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the saved level progress.
        /// </summary>
        /// <returns>The saved level, or MIN_LEVEL if not found.</returns>
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

        /// <summary>
        /// Generates a player report showing game statistics.
        /// </summary>
        /// <param name="playerName">The player name for the report.</param>
        public void GeneratePlayerReport(string playerName)
        {
            try
            {
                if (!File.Exists(FullLogPath))
                    return;

                var stats = CalculatePlayerStatistics();
                WritePlayerReport(playerName, stats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GeneratePlayerReport failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculates player statistics from the full log.
        /// </summary>
        /// <returns>A tuple containing wins, losses, and longest streak.</returns>
        private (long wins, long losses, long longestStreak) CalculatePlayerStatistics()
        {
            long wins = 0;
            long losses = 0;
            long currentStreak = 0;
            long longestStreak = 0;

            string[] lines = File.ReadAllLines(FullLogPath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Contains(RESULT_LABEL) && line.Contains(WIN_TEXT))
                {
                    wins++;
                    currentStreak++;
                    longestStreak = Math.Max(longestStreak, currentStreak);
                }
                else if (line.Contains(RESULT_LABEL) && line.Contains(LOSS_TEXT))
                {
                    losses++;
                    currentStreak = 0;
                }
            }

            return (wins, losses, longestStreak);
        }

        /// <summary>
        /// Writes the player report to file.
        /// </summary>
        /// <param name="playerName">The player name.</param>
        /// <param name="stats">The player statistics.</param>
        private void WritePlayerReport(string playerName, (long wins, long losses, long longestStreak) stats)
        {
            using (var writer = new StreamWriter(PlayerReportPath, false))
            {
                writer.WriteLine($"NES CPM Player Report for {playerName}");
                writer.WriteLine(DateTime.Now.ToLongDateString());
                writer.WriteLine();
                writer.WriteLine();

                long totalGames = stats.wins + stats.losses;
                writer.WriteLine($"      Games Played:  {totalGames}");
                writer.WriteLine($"              Wins:  {stats.wins}");
                writer.WriteLine($"Longest Win Streak:  {stats.longestStreak}");

                double winRate = totalGames > 0 ? (stats.wins / (double)totalGames) * 100.0 : 0.0;
                writer.WriteLine($"          Win Rate:  {winRate:F2}%");
                writer.WriteLine($"            Losses:  {stats.losses}");
                writer.WriteLine($"     Current Level:  {LoadLevel()}");
            }
        }

        /// <summary>
        /// Saves the player name.
        /// </summary>
        /// <param name="name">The player name to save.</param>
        public void SavePlayerName(string name)
        {
            _options.SavePlayerName(name);
        }

        /// <summary>
        /// Loads the player name.
        /// </summary>
        /// <returns>The saved player name.</returns>
        public string LoadPlayerName()
        {
            return _options.LoadPlayerName();
        }

        /// <summary>
        /// Saves the emulator path.
        /// </summary>
        /// <param name="path">The emulator path to save.</param>
        public void SaveEmulatorPath(string path)
        {
            _options.SaveEmulatorPath(path);
        }

        /// <summary>
        /// Loads the emulator path.
        /// </summary>
        /// <returns>The saved emulator path.</returns>
        public string LoadEmulatorPath()
        {
            return _options.LoadEmulatorPath();
        }

        /// <summary>
        /// Saves the text-to-speech setting.
        /// </summary>
        /// <param name="enabled">Whether text-to-speech is enabled.</param>
        public void SaveTextToSpeech(bool enabled)
        {
            _options.SaveTextToSpeech(enabled);
        }

        /// <summary>
        /// Loads the text-to-speech setting.
        /// </summary>
        /// <returns>Whether text-to-speech is enabled.</returns>
        public bool LoadTextToSpeech()
        {
            return _options.LoadTextToSpeech();
        }

        /// <summary>
        /// Saves the spoken timer interval (minutes) to options.
        /// </summary>
        /// <param name="minutes">Interval in minutes.</param>
        public void SaveTimerInterval(decimal minutes)
        {
            _options.TimerInterval = minutes;
            _options.Save();
        }

        /// <summary>
        /// Loads the spoken timer interval (minutes) from options.
        /// </summary>
        /// <returns>Interval in minutes.</returns>
        public decimal LoadTimerInterval()
        {
            return _options.LoadTextToSpeech() ? _options.TimerInterval : _options.TimerInterval; // just return stored value
        }

        /// <summary>
        /// Saves the start delay (minutes) to options.
        /// </summary>
        public void SaveStartDelay(decimal minutes)
        {
            _options.StartDelay = minutes;
            _options.Save();
        }

        /// <summary>
        /// Loads the start delay (minutes) from options.
        /// </summary>
        public decimal LoadStartDelay()
        {
            return _options.StartDelay;
        }

        /// <summary>
        /// Deletes the last game file if it exists.
        /// </summary>
        public void DeleteLastGame()
        {
            DeleteFileIfExists(LastGamePath);
        }

        /// <summary>
        /// Ensures the specified directory exists, creating it if necessary.
        /// </summary>
        /// <param name="path">The directory path to ensure.</param>
        private static void EnsureDirectoryExists(string path)
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to create directory: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Deletes a file if it exists, handling any exceptions silently.
        /// </summary>
        /// <param name="filePath">The file path to delete.</param>
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

        /// <summary>
        /// Atomically write content to a target file path by writing to a temp file then replacing the target.
        /// The writeAction should write to the provided stream (which will be closed by this method).
        /// </summary>
        /// <param name="targetPath">The file path to write to atomically.</param>
        /// <param name="writeAction">Action that writes content to the provided stream.</param>
        private void AtomicWrite(string targetPath, Action<Stream> writeAction)
        {
            var dir = Path.GetDirectoryName(targetPath);
            EnsureDirectoryExists(dir);

            string tempPath = null;
            try
            {
                tempPath = Path.Combine(dir, Path.GetFileName(targetPath) + ".tmp" + Guid.NewGuid().ToString("N"));
                // Write to temp file first
                using (var stream = File.Create(tempPath))
                {
                    writeAction(stream);
                }

                lock (_fileLock)
                {
                    if (File.Exists(targetPath))
                    {
                        // Replace preserves ACLs when available
                        File.Replace(tempPath, targetPath, null);
                    }
                    else
                    {
                        File.Move(tempPath, targetPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AtomicWrite failed for {targetPath}: {ex.Message}");
                try { if (tempPath != null && File.Exists(tempPath)) File.Delete(tempPath); } catch { }
            }
        }
    }
}