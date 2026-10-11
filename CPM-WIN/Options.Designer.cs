namespace CPM_WIN
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.HumanPlayerLBL = new System.Windows.Forms.Label();
            this.HumanNameTB = new System.Windows.Forms.TextBox();
            this.EmuPathLBL = new System.Windows.Forms.Label();
            this.EmuPathTB = new System.Windows.Forms.TextBox();
            this.BrowseBTN = new System.Windows.Forms.Button();
            this.TTSCHK = new System.Windows.Forms.CheckBox();
            this.TimerIntervalLBL = new System.Windows.Forms.Label();
            this.HelpTT = new System.Windows.Forms.ToolTip(this.components);
            this.OkBTN = new System.Windows.Forms.Button();
            this.CancelBTN = new System.Windows.Forms.Button();
            this.StartDelayLBL = new System.Windows.Forms.Label();
            this.TimerIntervalNUD = new System.Windows.Forms.NumericUpDown();
            this.StartDelayNUD = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.TimerIntervalNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDelayNUD)).BeginInit();
            this.SuspendLayout();
            // 
            // HumanPlayerLBL
            // 
            this.HumanPlayerLBL.AutoSize = true;
            this.HumanPlayerLBL.Location = new System.Drawing.Point(12, 21);
            this.HumanPlayerLBL.Name = "HumanPlayerLBL";
            this.HumanPlayerLBL.Size = new System.Drawing.Size(231, 36);
            this.HumanPlayerLBL.TabIndex = 0;
            this.HumanPlayerLBL.Text = "&Human Name:";
            this.HelpTT.SetToolTip(this.HumanPlayerLBL, "The name of the human player.  This name is randomly inserted into the list of pl" +
        "ayers in a game.  ");
            // 
            // HumanNameTB
            // 
            this.HumanNameTB.Location = new System.Drawing.Point(297, 18);
            this.HumanNameTB.Name = "HumanNameTB";
            this.HumanNameTB.Size = new System.Drawing.Size(386, 44);
            this.HumanNameTB.TabIndex = 1;
            // 
            // EmuPathLBL
            // 
            this.EmuPathLBL.AutoSize = true;
            this.EmuPathLBL.Location = new System.Drawing.Point(12, 97);
            this.EmuPathLBL.Name = "EmuPathLBL";
            this.EmuPathLBL.Size = new System.Drawing.Size(241, 36);
            this.EmuPathLBL.TabIndex = 2;
            this.EmuPathLBL.Text = "&Emulator Path:";
            this.HelpTT.SetToolTip(this.EmuPathLBL, "The path to an emulator executable.  This program starts when a new game begins, " +
        "and after the auto start timer is done. ");
            // 
            // EmuPathTB
            // 
            this.EmuPathTB.Location = new System.Drawing.Point(297, 94);
            this.EmuPathTB.Name = "EmuPathTB";
            this.EmuPathTB.ReadOnly = true;
            this.EmuPathTB.Size = new System.Drawing.Size(386, 44);
            this.EmuPathTB.TabIndex = 3;
            // 
            // BrowseBTN
            // 
            this.BrowseBTN.ForeColor = System.Drawing.Color.Black;
            this.BrowseBTN.Location = new System.Drawing.Point(690, 95);
            this.BrowseBTN.Name = "BrowseBTN";
            this.BrowseBTN.Size = new System.Drawing.Size(60, 43);
            this.BrowseBTN.TabIndex = 4;
            this.BrowseBTN.Text = "...";
            this.BrowseBTN.UseVisualStyleBackColor = true;
            this.BrowseBTN.Click += new System.EventHandler(this.BrowseBTN_Click);
            // 
            // TTSCHK
            // 
            this.TTSCHK.AutoSize = true;
            this.TTSCHK.Location = new System.Drawing.Point(12, 173);
            this.TTSCHK.Name = "TTSCHK";
            this.TTSCHK.Size = new System.Drawing.Size(513, 40);
            this.TTSCHK.TabIndex = 5;
            this.TTSCHK.Text = "&Text to Speech Announcements";
            this.HelpTT.SetToolTip(this.TTSCHK, "When checked, new game information and periodic timer announcements will be spoke" +
        "n.  ");
            this.TTSCHK.UseVisualStyleBackColor = true;
            // 
            // TimerIntervalLBL
            // 
            this.TimerIntervalLBL.AutoSize = true;
            this.TimerIntervalLBL.Location = new System.Drawing.Point(12, 253);
            this.TimerIntervalLBL.Name = "TimerIntervalLBL";
            this.TimerIntervalLBL.Size = new System.Drawing.Size(364, 36);
            this.TimerIntervalLBL.TabIndex = 6;
            this.TimerIntervalLBL.Text = "&Spoken Timer Interval:";
            this.TimerIntervalLBL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.HelpTT.SetToolTip(this.TimerIntervalLBL, "The game timer\'s elapsed time will speak every so often, based on the number of m" +
        "inutes selected.");
            // 
            // HelpTT
            // 
            this.HelpTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // OkBTN
            // 
            this.OkBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkBTN.ForeColor = System.Drawing.Color.Black;
            this.OkBTN.Location = new System.Drawing.Point(618, 410);
            this.OkBTN.Name = "OkBTN";
            this.OkBTN.Size = new System.Drawing.Size(132, 68);
            this.OkBTN.TabIndex = 11;
            this.OkBTN.Text = "&OK";
            this.HelpTT.SetToolTip(this.OkBTN, "Saves your settings ");
            this.OkBTN.UseVisualStyleBackColor = true;
            this.OkBTN.Click += new System.EventHandler(this.OkBTN_Click);
            // 
            // CancelBTN
            // 
            this.CancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBTN.ForeColor = System.Drawing.Color.Black;
            this.CancelBTN.Location = new System.Drawing.Point(12, 410);
            this.CancelBTN.Name = "CancelBTN";
            this.CancelBTN.Size = new System.Drawing.Size(132, 68);
            this.CancelBTN.TabIndex = 10;
            this.CancelBTN.Text = "&Cancel";
            this.HelpTT.SetToolTip(this.CancelBTN, "Discard changes.");
            this.CancelBTN.UseVisualStyleBackColor = true;
            this.CancelBTN.Click += new System.EventHandler(this.CancelBTN_Click);
            // 
            // StartDelayLBL
            // 
            this.StartDelayLBL.AutoSize = true;
            this.StartDelayLBL.Location = new System.Drawing.Point(12, 329);
            this.StartDelayLBL.Name = "StartDelayLBL";
            this.StartDelayLBL.Size = new System.Drawing.Size(424, 36);
            this.StartDelayLBL.TabIndex = 8;
            this.StartDelayLBL.Text = "&Delay before Timer Begins:";
            this.StartDelayLBL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.HelpTT.SetToolTip(this.StartDelayLBL, "This timer delays the start of the game timer and allows setting up the game with" +
        "out switching windows.  ");
            // 
            // TimerIntervalNUD
            // 
            this.TimerIntervalNUD.Location = new System.Drawing.Point(453, 245);
            this.TimerIntervalNUD.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.TimerIntervalNUD.Name = "TimerIntervalNUD";
            this.TimerIntervalNUD.Size = new System.Drawing.Size(95, 44);
            this.TimerIntervalNUD.TabIndex = 7;
            this.TimerIntervalNUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // StartDelayNUD
            // 
            this.StartDelayNUD.Location = new System.Drawing.Point(453, 327);
            this.StartDelayNUD.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.StartDelayNUD.Name = "StartDelayNUD";
            this.StartDelayNUD.Size = new System.Drawing.Size(95, 44);
            this.StartDelayNUD.TabIndex = 9;
            this.StartDelayNUD.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Options
            // 
            this.AcceptButton = this.OkBTN;
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 36F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.CancelButton = this.CancelBTN;
            this.ClientSize = new System.Drawing.Size(762, 484);
            this.Controls.Add(this.StartDelayNUD);
            this.Controls.Add(this.StartDelayLBL);
            this.Controls.Add(this.CancelBTN);
            this.Controls.Add(this.OkBTN);
            this.Controls.Add(this.TimerIntervalNUD);
            this.Controls.Add(this.TimerIntervalLBL);
            this.Controls.Add(this.TTSCHK);
            this.Controls.Add(this.BrowseBTN);
            this.Controls.Add(this.EmuPathTB);
            this.Controls.Add(this.EmuPathLBL);
            this.Controls.Add(this.HumanNameTB);
            this.Controls.Add(this.HumanPlayerLBL);
            this.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NES Monopoly Options";
            ((System.ComponentModel.ISupportInitialize)(this.TimerIntervalNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDelayNUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HumanPlayerLBL;
        private System.Windows.Forms.TextBox HumanNameTB;
        private System.Windows.Forms.Label EmuPathLBL;
        private System.Windows.Forms.TextBox EmuPathTB;
        private System.Windows.Forms.Button BrowseBTN;
        private System.Windows.Forms.CheckBox TTSCHK;
        private System.Windows.Forms.Label TimerIntervalLBL;
        private System.Windows.Forms.ToolTip HelpTT;
        private System.Windows.Forms.NumericUpDown TimerIntervalNUD;
        private System.Windows.Forms.Button OkBTN;
        private System.Windows.Forms.Button CancelBTN;
        private System.Windows.Forms.Label StartDelayLBL;
        private System.Windows.Forms.NumericUpDown StartDelayNUD;
    }
}