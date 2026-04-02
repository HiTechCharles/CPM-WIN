using System;
using System.IO;
using System.Windows.Forms;

namespace CPM_WIN
{
    public partial class ReportViewer : Form
    {
        public ReportViewer()  // Constructor
        {
            InitializeComponent();
            GetReportFilePath('F');  // Load the full log by default
            
        }

        public void SetControls(char fileType)
        {
            switch (fileType)
            {
                case 'F':
                    fullLogToolStripMenuItem.Checked = true;
                    fullLogToolStripMenuItem.AccessibleName = "Full Log checked.";
                    lastGameToolStripMenuItem.Checked = false;
                    lastGameToolStripMenuItem.AccessibleName = "Last Game unchecked.";
                    playerreportToolStripMenuItem.Checked = false;
                    playerreportToolStripMenuItem.AccessibleName = "Player Report unchecked.";
                    break;
                case 'L':
                    fullLogToolStripMenuItem.Checked = false;
                    fullLogToolStripMenuItem.AccessibleName = "Full Log unchecked.";
                    lastGameToolStripMenuItem.Checked = true;
                    lastGameToolStripMenuItem.AccessibleName = "Last Game checked.";
                    playerreportToolStripMenuItem.Checked = false;
                    playerreportToolStripMenuItem.AccessibleName = "Player Report unchecked.";
                    break;
                case 'P':
                    fullLogToolStripMenuItem.Checked = false;
                    fullLogToolStripMenuItem.AccessibleName = "Full Log unchecked.";
                    lastGameToolStripMenuItem.Checked = false;
                    lastGameToolStripMenuItem.AccessibleName = "Last Game unchecked.";
                    playerreportToolStripMenuItem.Checked = true;
                    playerreportToolStripMenuItem.AccessibleName = "Player Report checked.";                
                    break;
                default:
                    throw new ArgumentException("Invalid file type");
            }
        }

        public void GetReportFilePath(char fileType)  // Method to get report file path based on file type
        {
            string filePath;
            switch (fileType)  // Switch statement to determine file type
            {
                case 'F':  // Full Log
                    filePath = Form1.FullLog;
                    break;
                case 'L':  // Last Game
                    filePath = Form1.LastGame;
                    break;
                case 'P':  // Player Report
                    filePath = Form1.PlayerReport;
                    break;
                default:
                    throw new ArgumentException("Invalid file type");
            }
            SetControls(fileType);  // Update menu item states

            try
            {
                ReportRTB.Clear();  // Clear the RichTextBox before loading new content
                using (StreamReader sr = new StreamReader(filePath))
                {
                    ReportRTB.Text = sr.ReadToEnd();  // Read and display the content of the file
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading file: " + ex.Message);
            }
        }

        #region Menu Strip
        private void fullLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetReportFilePath('F');
        }

        private void lastGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetReportFilePath('L');
        }

        private void playerreportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetReportFilePath('P');
        }

        private void readSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.synth.SpeakAsyncCancelAll();
            foreach (var line in ReportRTB.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                Form1.synth.SpeakAsync(line + ".");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}