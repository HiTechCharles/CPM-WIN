namespace CPM_WIN
{
    partial class Form1
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
            this.DateTimeLBL = new System.Windows.Forms.Label();
            this.DateTimeTB = new System.Windows.Forms.TextBox();
            this.NUMCPUTB = new System.Windows.Forms.TextBox();
            this.NumCPULBL = new System.Windows.Forms.Label();
            this.NamesTB = new System.Windows.Forms.TextBox();
            this.NamesLBL = new System.Windows.Forms.Label();
            this.GameRuleTB = new System.Windows.Forms.TextBox();
            this.GameRuleLBL = new System.Windows.Forms.Label();
            this.GameTimeTB = new System.Windows.Forms.TextBox();
            this.GameTimeLBL = new System.Windows.Forms.Label();
            this.AssetsLBL = new System.Windows.Forms.Label();
            this.AssetsNUD = new System.Windows.Forms.NumericUpDown();
            this.MainMenuMST = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stoPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.AssetsNUD)).BeginInit();
            this.MainMenuMST.SuspendLayout();
            this.SuspendLayout();
            // 
            // DateTimeLBL
            // 
            this.DateTimeLBL.AutoSize = true;
            this.DateTimeLBL.Location = new System.Drawing.Point(21, 65);
            this.DateTimeLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DateTimeLBL.Name = "DateTimeLBL";
            this.DateTimeLBL.Size = new System.Drawing.Size(170, 29);
            this.DateTimeLBL.TabIndex = 1;
            this.DateTimeLBL.Text = "&Date && Time:";
            // 
            // DateTimeTB
            // 
            this.DateTimeTB.Location = new System.Drawing.Point(291, 59);
            this.DateTimeTB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DateTimeTB.Name = "DateTimeTB";
            this.DateTimeTB.ReadOnly = true;
            this.DateTimeTB.Size = new System.Drawing.Size(457, 36);
            this.DateTimeTB.TabIndex = 2;
            // 
            // NUMCPUTB
            // 
            this.NUMCPUTB.Location = new System.Drawing.Point(291, 130);
            this.NUMCPUTB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NUMCPUTB.Name = "NUMCPUTB";
            this.NUMCPUTB.ReadOnly = true;
            this.NUMCPUTB.Size = new System.Drawing.Size(457, 36);
            this.NUMCPUTB.TabIndex = 4;
            // 
            // NumCPULBL
            // 
            this.NumCPULBL.AutoSize = true;
            this.NumCPULBL.Location = new System.Drawing.Point(21, 136);
            this.NumCPULBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NumCPULBL.Name = "NumCPULBL";
            this.NumCPULBL.Size = new System.Drawing.Size(193, 29);
            this.NumCPULBL.TabIndex = 3;
            this.NumCPULBL.Text = "# &CPU Players:";
            // 
            // NamesTB
            // 
            this.NamesTB.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.NamesTB.Location = new System.Drawing.Point(291, 199);
            this.NamesTB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NamesTB.Multiline = true;
            this.NamesTB.Name = "NamesTB";
            this.NamesTB.ReadOnly = true;
            this.NamesTB.Size = new System.Drawing.Size(457, 79);
            this.NamesTB.TabIndex = 6;
            // 
            // NamesLBL
            // 
            this.NamesLBL.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.NamesLBL.AutoSize = true;
            this.NamesLBL.Location = new System.Drawing.Point(21, 242);
            this.NamesLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NamesLBL.Name = "NamesLBL";
            this.NamesLBL.Size = new System.Drawing.Size(186, 29);
            this.NamesLBL.TabIndex = 5;
            this.NamesLBL.Text = "&Player Names:";
            // 
            // GameRuleTB
            // 
            this.GameRuleTB.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.GameRuleTB.Location = new System.Drawing.Point(291, 312);
            this.GameRuleTB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GameRuleTB.Multiline = true;
            this.GameRuleTB.Name = "GameRuleTB";
            this.GameRuleTB.ReadOnly = true;
            this.GameRuleTB.Size = new System.Drawing.Size(457, 76);
            this.GameRuleTB.TabIndex = 8;
            // 
            // GameRuleLBL
            // 
            this.GameRuleLBL.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.GameRuleLBL.AutoSize = true;
            this.GameRuleLBL.Location = new System.Drawing.Point(21, 352);
            this.GameRuleLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GameRuleLBL.Name = "GameRuleLBL";
            this.GameRuleLBL.Size = new System.Drawing.Size(151, 29);
            this.GameRuleLBL.TabIndex = 7;
            this.GameRuleLBL.Text = "&Game Rule:";
            // 
            // GameTimeTB
            // 
            this.GameTimeTB.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.GameTimeTB.Location = new System.Drawing.Point(291, 430);
            this.GameTimeTB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GameTimeTB.Name = "GameTimeTB";
            this.GameTimeTB.ReadOnly = true;
            this.GameTimeTB.Size = new System.Drawing.Size(457, 36);
            this.GameTimeTB.TabIndex = 10;
            // 
            // GameTimeLBL
            // 
            this.GameTimeLBL.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.GameTimeLBL.AutoSize = true;
            this.GameTimeLBL.Location = new System.Drawing.Point(21, 436);
            this.GameTimeLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GameTimeLBL.Name = "GameTimeLBL";
            this.GameTimeLBL.Size = new System.Drawing.Size(180, 29);
            this.GameTimeLBL.TabIndex = 9;
            this.GameTimeLBL.Text = "&Elapsed Time:";
            // 
            // AssetsLBL
            // 
            this.AssetsLBL.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AssetsLBL.AutoSize = true;
            this.AssetsLBL.Location = new System.Drawing.Point(21, 517);
            this.AssetsLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AssetsLBL.Name = "AssetsLBL";
            this.AssetsLBL.Size = new System.Drawing.Size(166, 29);
            this.AssetsLBL.TabIndex = 11;
            this.AssetsLBL.Text = "Total &Assets:";
            // 
            // AssetsNUD
            // 
            this.AssetsNUD.Location = new System.Drawing.Point(291, 511);
            this.AssetsNUD.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.AssetsNUD.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.AssetsNUD.Name = "AssetsNUD";
            this.AssetsNUD.Size = new System.Drawing.Size(457, 36);
            this.AssetsNUD.TabIndex = 12;
            // 
            // MainMenuMST
            // 
            this.MainMenuMST.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.MainMenuMST.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenuMST.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.timerToolStripMenuItem});
            this.MainMenuMST.Location = new System.Drawing.Point(0, 0);
            this.MainMenuMST.Name = "MainMenuMST";
            this.MainMenuMST.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.MainMenuMST.Size = new System.Drawing.Size(761, 37);
            this.MainMenuMST.TabIndex = 0;
            this.MainMenuMST.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.readToolStripMenuItem,
            this.viewRecordsToolStripMenuItem,
            this.saveExitToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(117, 33);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.newGameToolStripMenuItem.Text = "&New Game";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // readToolStripMenuItem
            // 
            this.readToolStripMenuItem.Name = "readToolStripMenuItem";
            this.readToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.readToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.readToolStripMenuItem.Text = "&Read";
            this.readToolStripMenuItem.Click += new System.EventHandler(this.readToolStripMenuItem_Click);
            // 
            // viewRecordsToolStripMenuItem
            // 
            this.viewRecordsToolStripMenuItem.Name = "viewRecordsToolStripMenuItem";
            this.viewRecordsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.viewRecordsToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.viewRecordsToolStripMenuItem.Text = "Report &Viewer";
            this.viewRecordsToolStripMenuItem.Click += new System.EventHandler(this.viewRecordsToolStripMenuItem_Click);
            // 
            // saveExitToolStripMenuItem
            // 
            this.saveExitToolStripMenuItem.Name = "saveExitToolStripMenuItem";
            this.saveExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.saveExitToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.saveExitToolStripMenuItem.Text = "&Save && Exit";
            this.saveExitToolStripMenuItem.Click += new System.EventHandler(this.saveExitToolStripMenuItem_Click);
            // 
            // timerToolStripMenuItem
            // 
            this.timerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.startToolStripMenuItem,
            this.stoPToolStripMenuItem});
            this.timerToolStripMenuItem.Name = "timerToolStripMenuItem";
            this.timerToolStripMenuItem.Size = new System.Drawing.Size(94, 33);
            this.timerToolStripMenuItem.Text = "&Timer";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(246, 34);
            this.resetToolStripMenuItem.Text = "&Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.startToolStripMenuItem.Size = new System.Drawing.Size(246, 34);
            this.startToolStripMenuItem.Text = "&Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stoPToolStripMenuItem
            // 
            this.stoPToolStripMenuItem.Name = "stoPToolStripMenuItem";
            this.stoPToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.stoPToolStripMenuItem.Size = new System.Drawing.Size(246, 34);
            this.stoPToolStripMenuItem.Text = "St&op";
            this.stoPToolStripMenuItem.Click += new System.EventHandler(this.stoPToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(761, 583);
            this.Controls.Add(this.AssetsNUD);
            this.Controls.Add(this.AssetsLBL);
            this.Controls.Add(this.GameTimeTB);
            this.Controls.Add(this.GameTimeLBL);
            this.Controls.Add(this.GameRuleTB);
            this.Controls.Add(this.GameRuleLBL);
            this.Controls.Add(this.NamesTB);
            this.Controls.Add(this.NamesLBL);
            this.Controls.Add(this.NUMCPUTB);
            this.Controls.Add(this.NumCPULBL);
            this.Controls.Add(this.DateTimeTB);
            this.Controls.Add(this.DateTimeLBL);
            this.Controls.Add(this.MainMenuMST);
            this.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.MainMenuStrip = this.MainMenuMST;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Computer Picker for NES Monopoly";
            ((System.ComponentModel.ISupportInitialize)(this.AssetsNUD)).EndInit();
            this.MainMenuMST.ResumeLayout(false);
            this.MainMenuMST.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DateTimeLBL;
        private System.Windows.Forms.TextBox DateTimeTB;
        private System.Windows.Forms.TextBox NUMCPUTB;
        private System.Windows.Forms.Label NumCPULBL;
        private System.Windows.Forms.TextBox NamesTB;
        private System.Windows.Forms.Label NamesLBL;
        private System.Windows.Forms.TextBox GameRuleTB;
        private System.Windows.Forms.Label GameRuleLBL;
        private System.Windows.Forms.TextBox GameTimeTB;
        private System.Windows.Forms.Label GameTimeLBL;
        private System.Windows.Forms.Label AssetsLBL;
        private System.Windows.Forms.NumericUpDown AssetsNUD;
        private System.Windows.Forms.MenuStrip MainMenuMST;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stoPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewRecordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
    }
}

