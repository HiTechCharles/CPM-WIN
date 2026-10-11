using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace CPM_WIN
{
    /// <summary>
    /// Manages application options including emulator path, player name, and text-to-speech preference.
    /// Options are saved to and loaded from Options.xml in the application directory.
    /// </summary>
    public class OptionsManager
    {
        private const string OPTIONS_FILE = "Options.xml";
        private const bool DEFAULT_TEXT_TO_SPEECH = true;

        private readonly string _appDirectory;
        private readonly string _optionsFilePath;
        private bool _disposed;

        /// <summary>
        /// Gets or sets the emulator path.
        /// </summary>
        public string EmulatorPath { get; set; }

        /// <summary>
        /// Gets or sets the player name.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether text-to-speech is enabled.
        /// </summary>
        public bool TextToSpeechEnabled { get; set; }

        /// <summary>
        /// Gets or sets the timer interval (minutes) for spoken announcements.
        /// </summary>
        public decimal TimerInterval { get; set; }

        /// <summary>
        /// Gets or sets the start delay (minutes) before auto-start.
        /// </summary>
        public decimal StartDelay { get; set; }

        /// <summary>
        /// DTO used for XML serialization of options.
        /// </summary>
        public class OptionsDto
        {
            public string EmulatorPath { get; set; }
            public string PlayerName { get; set; }
            public bool TextToSpeechEnabled { get; set; } = DEFAULT_TEXT_TO_SPEECH;
            public decimal TimerInterval { get; set; } = 10m;
            public decimal StartDelay { get; set; } = 0m;
        }

        /// <summary>
        /// Initializes a new instance of the OptionsManager class.
        /// </summary>
        /// <param name="appDirectory">The application directory where Options.xml will be stored.</param>
        /// <exception cref="ArgumentNullException">Thrown when appDirectory is null or whitespace.</exception>
        public OptionsManager(string appDirectory)
        {
            if (string.IsNullOrWhiteSpace(appDirectory))
            {
                throw new ArgumentNullException(nameof(appDirectory));
            }

            _appDirectory = appDirectory;
            _optionsFilePath = Path.Combine(appDirectory, OPTIONS_FILE);

            EmulatorPath = string.Empty;
            PlayerName = string.Empty;
            TextToSpeechEnabled = DEFAULT_TEXT_TO_SPEECH;
            TimerInterval = 10m;
            StartDelay = 0m;
            _disposed = false;
        }

        /// <summary>
        /// Loads options from the Options.xml file.
        /// </summary>
        public void Load()
        {
            if (_disposed)
                return;

            try
            {
                if (!File.Exists(_optionsFilePath))
                    return;

                var serializer = new XmlSerializer(typeof(OptionsDto));
                using (var stream = File.OpenRead(_optionsFilePath))
                {
                    if (serializer.Deserialize(stream) is OptionsDto dto)
                    {
                        EmulatorPath = dto.EmulatorPath ?? string.Empty;
                        PlayerName = dto.PlayerName ?? string.Empty;
                        TextToSpeechEnabled = dto.TextToSpeechEnabled;
                        TimerInterval = dto.TimerInterval;
                        StartDelay = dto.StartDelay;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadOptions failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves options to the Options.xml file atomically.
        /// </summary>
        public void Save()
        {
            if (_disposed)
                return;

            try
            {
                EnsureDirectoryExists(_appDirectory);

                var dto = new OptionsDto
                {
                    EmulatorPath = EmulatorPath,
                    PlayerName = PlayerName,
                    TextToSpeechEnabled = TextToSpeechEnabled
                    ,
                    TimerInterval = TimerInterval,
                    StartDelay = StartDelay
                };

                var serializer = new XmlSerializer(typeof(OptionsDto));
                string tempFile = _optionsFilePath + ".tmp";

                using (var stream = File.Create(tempFile))
                {
                    serializer.Serialize(stream, dto);
                    stream.Flush();
                }

                if (File.Exists(_optionsFilePath))
                {
                    // Replace existing file with the temp file atomically
                    File.Replace(tempFile, _optionsFilePath, null);
                }
                else
                {
                    File.Move(tempFile, _optionsFilePath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveOptions failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the player name to options.
        /// </summary>
        /// <param name="name">The player name to save.</param>
        public void SavePlayerName(string name)
        {
            if (_disposed)
                return;

            PlayerName = name;
            Save();
        }

        /// <summary>
        /// Loads the player name from options.
        /// </summary>
        /// <returns>The saved player name, or empty string if not found.</returns>
        public string LoadPlayerName()
        {
            Load();
            return PlayerName ?? string.Empty;
        }

        /// <summary>
        /// Saves the emulator path to options.
        /// </summary>
        /// <param name="path">The emulator path to save.</param>
        public void SaveEmulatorPath(string path)
        {
            if (_disposed)
                return;

            EmulatorPath = path;
            Save();
        }

        /// <summary>
        /// Loads the emulator path from options.
        /// </summary>
        /// <returns>The saved emulator path, or empty string if not found.</returns>
        public string LoadEmulatorPath()
        {
            Load();
            return EmulatorPath ?? string.Empty;
        }

        /// <summary>
        /// Saves the text-to-speech enabled flag.
        /// </summary>
        /// <param name="enabled">Whether text-to-speech should be enabled.</param>
        public void SaveTextToSpeech(bool enabled)
        {
            if (_disposed)
                return;

            TextToSpeechEnabled = enabled;
            Save();
        }

        /// <summary>
        /// Loads the text-to-speech enabled flag.
        /// </summary>
        /// <returns>True if text-to-speech is enabled; otherwise false.</returns>
        public bool LoadTextToSpeech()
        {
            Load();
            return TextToSpeechEnabled;
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
        /// Releases all resources used by the OptionsManager.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }
}
