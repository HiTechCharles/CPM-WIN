using System;
using System.IO;
using System.Windows.Forms;

namespace CPM_WIN
{
    public partial class ReportViewer : Form
    {
        private const char FILE_TYPE_FULL_LOG = 'F';
        private const char FILE_TYPE_LAST_GAME = 'L';
        private const char FILE_TYPE_PLAYER_REPORT = 'P';

        public ReportViewer()
        {
            InitializeComponent();
            LoadReport(FILE_TYPE_FULL_LOG);
        }

        private void SetMenuCheckStates(char fileType)
        {
            fullLogToolStripMenuItem.Checked = fileType == FILE_TYPE_FULL_LOG;
            fullLogToolStripMenuItem.AccessibleName = $"Full Log {(fileType == FILE_TYPE_FULL_LOG ? "checked" : "unchecked")}.";

            lastGameToolStripMenuItem.Checked = fileType == FILE_TYPE_LAST_GAME;
            lastGameToolStripMenuItem.AccessibleName = $"Last Game {(fileType == FILE_TYPE_LAST_GAME ? "checked" : "unchecked")}.";

            playerreportToolStripMenuItem.Checked = fileType == FILE_TYPE_PLAYER_REPORT;
            playerreportToolStripMenuItem.AccessibleName = $"Player Report {(fileType == FILE_TYPE_PLAYER_REPORT ? "checked" : "unchecked")}.";
        }

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
                    ReportRTB.Text = $"File not found: {Path.GetFileName(filePath)}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}", "File Read Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReportRTB.Text = $"Error: {ex.Message}";
            }
        }

        #region Menu Event Handlers
        private void fullLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadReport(FILE_TYPE_FULL_LOG);
        }

        private void lastGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadReport(FILE_TYPE_LAST_GAME);
        }

        private void playerreportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadReport(FILE_TYPE_PLAYER_REPORT);
        }

        private void readSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.CancelAllSpeech();

                string[] lines = ReportRTB.Text.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string textToSpeak = line.TrimEnd();
                        if (!textToSpeak.EndsWith(".") && !textToSpeak.EndsWith("!") && !textToSpeak.EndsWith("?"))
                        {
                            textToSpeak += ".";
                        }

                        Form1.SpeakText(textToSpeak);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading text: {ex.Message}", "Speech Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }
}