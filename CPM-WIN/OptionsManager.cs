using System;
using System.Diagnostics;
using System.IO;

namespace CPM_WIN
{
    /// <summary>
    /// Manages application options including emulator path, player name, and text-to-speech preference.
    /// Options are saved to and loaded from options.txt in the application directory.
    /// </summary>
    public class OptionsManager
    {
        private const string OPTIONS_FILE = "Options.txt";
        private const string EMULATOR_PATH_KEY = "EmulatorPath";
        private const string PLAYER_NAME_KEY = "PlayerName";
        private const string TEXT_TO_SPEECH_KEY = "TextToSpeech";
        private const char KEY_VALUE_SEPARATOR = '=';
        private const int EXPECTED_PARTS_COUNT = 2;
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
        /// Initializes a new instance of the OptionsManager class.
        /// </summary>
        /// <param name="appDirectory">The application directory where options.txt will be stored.</param>
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
            _disposed = false;
        }

        /// <summary>
        /// Loads options from the options.txt file.
        /// </summary>
        public void Load()
        {
            if (_disposed)
                return;

            try
            {
                if (!File.Exists(_optionsFilePath))
                {
                    return;
                }

                string[] lines = File.ReadAllLines(_optionsFilePath);
                ParseOptionsFromLines(lines);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadOptions failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Parses options from an array of configuration lines.
        /// </summary>
        /// <param name="lines">The lines to parse.</param>
        private void ParseOptionsFromLines(string[] lines)
        {
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] parts = line.Split(new[] { KEY_VALUE_SEPARATOR }, EXPECTED_PARTS_COUNT);
                if (parts.Length != EXPECTED_PARTS_COUNT)
                {
                    continue;
                }

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                switch (key)
                {
                    case EMULATOR_PATH_KEY:
                        EmulatorPath = value;
                        break;
                    case PLAYER_NAME_KEY:
                        PlayerName = value;
                        break;
                    case TEXT_TO_SPEECH_KEY:
                        if (bool.TryParse(value, out bool enabled))
                            TextToSpeechEnabled = enabled;
                        break;
                }
            }
        }

        /// <summary>
        /// Saves options to the options.txt file.
        /// </summary>
        public void Save()
        {
            if (_disposed)
                return;

            try
            {
                EnsureDirectoryExists(_appDirectory);

                using (var writer = new StreamWriter(_optionsFilePath, false))
                {
                    // Always write TextToSpeech setting so it's preserved even when false
                    writer.WriteLine($"{TEXT_TO_SPEECH_KEY}={TextToSpeechEnabled}");

                    if (!string.IsNullOrEmpty(EmulatorPath))
                    {
                        writer.WriteLine($"{EMULATOR_PATH_KEY}={EmulatorPath}");
                    }

                    if (!string.IsNullOrEmpty(PlayerName))
                    {
                        writer.WriteLine($"{PLAYER_NAME_KEY}={PlayerName}");
                    }
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
