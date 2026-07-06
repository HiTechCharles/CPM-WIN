using System;
using System.IO;
using System.Windows.Forms;

namespace CPM_WIN
{
    /// <summary>
    /// Displays and manages reading of game reports (full log, last game, player report).
    /// </summary>
    public partial class ReportViewer : Form
    {
        private const char FILE_TYPE_FULL_LOG = 'F';
        private const char FILE_TYPE_LAST_GAME = 'L';
        private const char FILE_TYPE_PLAYER_REPORT = 'P';
        private const string CHECKED_STATE = "checked";
        private const string UNCHECKED_STATE = "unchecked";
        private const string FILE_NOT_FOUND_FORMAT = "File not found: {0}";
        private const string ERROR_READING_FILE = "Error reading file: {0}";
        private const string ERROR_READING_TEXT = "Error reading text: {0}";
        private const string SENTENCE_ENDINGS = ".!?";

        /// <summary>
        /// Initializes a new instance of the ReportViewer form.
        /// </summary>
        public ReportViewer()
        {
            InitializeComponent();
            LoadReport(FILE_TYPE_FULL_LOG);
        }

        /// <summary>
        /// Sets the menu check states based on the current file type.
        /// </summary>
        /// <param name="fileType">The current file type being viewed.</param>
        private void SetMenuCheckStates(char fileType)
        {
            bool isFullLog = fileType == FILE_TYPE_FULL_LOG;
            bool isLastGame = fileType == FILE_TYPE_LAST_GAME;
            bool isPlayerReport = fileType == FILE_TYPE_PLAYER_REPORT;

            fullLogToolStripMenuItem.Checked = isFullLog;
            fullLogToolStripMenuItem.AccessibleName = $"Full Log {(isFullLog ? CHECKED_STATE : UNCHECKED_STATE)}.";

            lastGameToolStripMenuItem.Checked = isLastGame;
            lastGameToolStripMenuItem.AccessibleName = $"Last Game {(isLastGame ? CHECKED_STATE : UNCHECKED_STATE)}.";

            playerreportToolStripMenuItem.Checked = isPlayerReport;
            playerreportToolStripMenuItem.AccessibleName = $"Player Report {(isPlayerReport ? CHECKED_STATE : UNCHECKED_STATE)}.";
        }

        /// <summary>
        /// Gets the file path for the specified file type.
        /// </summary>
        /// <param name="fileType">The type of file to retrieve.</param>
        /// <returns>The full path to the requested file.</returns>
        /// <exception cref="ArgumentException">Thrown when fileType is not valid.</exception>
        private string GetFilePath(char fileType)
        {
            switch (fileType)
            {
                case FILE_TYPE_FULL_LOG:
                    return Form1.FullLogPath;
                case FILE_TYPE_LAST_GAME:
                    return Form1.LastGamePath;
                case FILE_TYPE_PLAYER_REPORT:
                    return Form1.PlayerReportPath;
                default:
                    throw new ArgumentException($"Invalid file type: {fileType}", nameof(fileType));
            }
        }

        /// <summary>
        /// Loads and displays the report for the specified file type.
        /// </summary>
        /// <param name="fileType">The type of report to load.</param>
        private void LoadReport(char fileType)
        {
            string filePath = GetFilePath(fileType);
            SetMenuCheckStates(fileType);

            try
            {
                ReportRTB.Clear();

                if (File.Exists(filePath))
                {
                    ReportRTB.Text = File.ReadAllText(filePath);
                }
                else
                {
                    ReportRTB.Text = string.Format(FILE_NOT_FOUND_FORMAT, Path.GetFileName(filePath));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(ERROR_READING_FILE, ex.Message), "File Read Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReportRTB.Text = $"Error: {ex.Message}";
            }
        }

        #region Menu Event Handlers
        /// <summary>
        /// Handles the full log menu item click event.
        /// </summary>
        private void fullLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadReport(FILE_TYPE_FULL_LOG);
        }

        /// <summary>
        /// Handles the last game menu item click event.
        /// </summary>
        private void lastGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadReport(FILE_TYPE_LAST_GAME);
        }

        /// <summary>
        /// Handles the player report menu item click event.
        /// </summary>
        private void playerreportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadReport(FILE_TYPE_PLAYER_REPORT);
        }

        /// <summary>
        /// Reads the selected text from the report using text-to-speech.
        /// </summary>
        private void readSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.CancelAllSpeech();
                SpeakReportText();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(ERROR_READING_TEXT, ex.Message), "Speech Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Speaks each line of the report text.
        /// </summary>
        private void SpeakReportText()
        {
            string[] lines = ReportRTB.Text.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string textToSpeak = PrepareLineForSpeech(line);
                Form1.SpeakText(textToSpeak);
            }
        }

        /// <summary>
        /// Prepares a line of text for speech by trimming and ensuring proper sentence endings.
        /// </summary>
        /// <param name="line">The line to prepare.</param>
        /// <returns>The prepared line with proper formatting.</returns>
        private static string PrepareLineForSpeech(string line)
        {
            string textToSpeak = line.TrimEnd();
            if (!HasSentenceEnding(textToSpeak))
            {
                textToSpeak += ".";
            }
            return textToSpeak;
        }

        /// <summary>
        /// Checks if a line ends with a sentence-ending punctuation mark.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <returns>True if the text ends with proper punctuation; otherwise false.</returns>
        private static bool HasSentenceEnding(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            char lastChar = text[text.Length - 1];
            return SENTENCE_ENDINGS.Contains(lastChar.ToString());
        }

        /// <summary>
        /// Handles the exit menu item click event.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }
}