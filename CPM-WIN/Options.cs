using System;
using System.Drawing;
using System.Windows.Forms;

namespace CPM_WIN
{
    public partial class Options : Form
    {
        string _HumanName;
        string _EmulatorPath;
        bool _TTS;
        decimal _TimerInterval;
        decimal _DelayInterval;
        private readonly LoggingManager _loggingManager;

        public Options(LoggingManager loggingManager)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            InitializeComponent();
            SetupAccessibility();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Load current settings so the dialog shows existing values
            try
            {
                HumanNameTB.Text = _loggingManager.LoadPlayerName();
                EmuPathTB.Text = _loggingManager.LoadEmulatorPath();
                TTSCHK.Checked = _loggingManager.LoadTextToSpeech();
                try { TimerIntervalNUD.Value = _loggingManager.LoadTimerInterval(); } catch { }
                try { StartDelayNUD.Value = _loggingManager.LoadStartDelay(); } catch { }
            }
            catch
            {
                // suppress load errors
            }
        }

        private void CheckHumanName()
        {
            if (string.IsNullOrWhiteSpace(HumanNameTB.Text))
            {
                HumanNameTB.BackColor = Color.Red;
                OkBTN.Enabled = false;
            }
            else
            {
                HumanNameTB.BackColor = Color.White;
                OkBTN.Enabled = true;
            }
        }

        private void OkBTN_Click(object sender, EventArgs e)
        {
            CheckHumanName();
            _HumanName = HumanNameTB.Text;
            _EmulatorPath = EmuPathTB.Text;
            _TTS = TTSCHK.Checked;
            _TimerInterval = TimerIntervalNUD.Value;
            _DelayInterval = StartDelayNUD.Value;

            // Persist options to disk using LoggingManager/OptionsManager
            try
            {
                _loggingManager.SavePlayerName(_HumanName);
                _loggingManager.SaveEmulatorPath(_EmulatorPath);
                _loggingManager.SaveTextToSpeech(_TTS);
                _loggingManager.SaveTimerInterval(_TimerInterval);
                _loggingManager.SaveStartDelay(_DelayInterval);

                // Update running form's human name and start delay if available
                try { Form1.SetHumanPlayerName(_HumanName); Form1.SetStartDelay(_DelayInterval); } catch { }
            }
            catch
            {
                // suppress save errors
            }
        }

        private void CancelBTN_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BrowseBTN_Click(object sender, EventArgs e)
        {
            FileDialog FD = new OpenFileDialog();
            FD.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
            if (FD.ShowDialog() == DialogResult.OK)
            {
                EmuPathTB.Text = FD.FileName;
            }
        }

        private void SetupAccessibility()
        {
            // Set accessibility properties for controls
            HumanNameTB.AccessibleName = "Human Player Name";
            EmuPathTB.AccessibleName = "Emulator Path";
            TTSCHK.AccessibleName = "Enable Text-to-Speech";
            TimerIntervalNUD.AccessibleName = "Spoken Timer Interval";
            StartDelayNUD.AccessibleName = "Delay before Timer Starts";
            OkBTN.AccessibleName = "OK";
            CancelBTN.AccessibleName = "Cancel";
            BrowseBTN.AccessibleName = "Browse";
        }
    }
}
