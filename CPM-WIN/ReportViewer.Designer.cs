namespace CPM_WIN
{
    partial class ReportViewer
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
            this.ReportRTB = new System.Windows.Forms.RichTextBox();
            this.MainMenuMST = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerreportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuMST.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReportRTB
            // 
            this.ReportRTB.Font = new System.Drawing.Font("Cascadia Mono", 16F, System.Drawing.FontStyle.Bold);
            this.ReportRTB.Location = new System.Drawing.Point(11, 51);
            this.ReportRTB.Name = "ReportRTB";
            this.ReportRTB.ReadOnly = true;
            this.ReportRTB.Size = new System.Drawing.Size(855, 435);
            this.ReportRTB.TabIndex = 0;
            this.ReportRTB.Text = "";
            // 
            // MainMenuMST
            // 
            this.MainMenuMST.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.MainMenuMST.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenuMST.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.MainMenuMST.Location = new System.Drawing.Point(0, 0);
            this.MainMenuMST.Name = "MainMenuMST";
            this.MainMenuMST.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MainMenuMST.Size = new System.Drawing.Size(876, 37);
            this.MainMenuMST.TabIndex = 0;
            this.MainMenuMST.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullLogToolStripMenuItem,
            this.lastGameToolStripMenuItem,
            this.playerreportToolStripMenuItem,
            this.readSelectedToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(67, 33);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // fullLogToolStripMenuItem
            // 
            this.fullLogToolStripMenuItem.Name = "fullLogToolStripMenuItem";
            this.fullLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fullLogToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.fullLogToolStripMenuItem.Text = "&Full Log";
            this.fullLogToolStripMenuItem.Click += new System.EventHandler(this.fullLogToolStripMenuItem_Click);
            // 
            // lastGameToolStripMenuItem
            // 
            this.lastGameToolStripMenuItem.Name = "lastGameToolStripMenuItem";
            this.lastGameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.lastGameToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.lastGameToolStripMenuItem.Text = "&Last Game";
            this.lastGameToolStripMenuItem.Click += new System.EventHandler(this.lastGameToolStripMenuItem_Click);
            // 
            // playerreportToolStripMenuItem
            // 
            this.playerreportToolStripMenuItem.Name = "playerreportToolStripMenuItem";
            this.playerreportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.playerreportToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.playerreportToolStripMenuItem.Text = "&Player Report";
            this.playerreportToolStripMenuItem.Click += new System.EventHandler(this.playerreportToolStripMenuItem_Click);
            // 
            // readSelectedToolStripMenuItem
            // 
            this.readSelectedToolStripMenuItem.Name = "readSelectedToolStripMenuItem";
            this.readSelectedToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.readSelectedToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.readSelectedToolStripMenuItem.Text = "&Read Selected";
            this.readSelectedToolStripMenuItem.Click += new System.EventHandler(this.readSelectedToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(348, 34);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkBlue;
            this.ClientSize = new System.Drawing.Size(876, 499);
            this.Controls.Add(this.ReportRTB);
            this.Controls.Add(this.MainMenuMST);
            this.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.MainMenuStrip = this.MainMenuMST;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CPM Report Viewer";
            this.MainMenuMST.ResumeLayout(false);
            this.MainMenuMST.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ReportRTB;
        private System.Windows.Forms.MenuStrip MainMenuMST;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerreportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}