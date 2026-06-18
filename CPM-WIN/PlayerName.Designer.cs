namespace CPM_WIN
{
    partial class PlayerName
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
            this.PlayerNameTB = new System.Windows.Forms.TextBox();
            this.OkBTN = new System.Windows.Forms.Button();
            this.CancelBTN = new System.Windows.Forms.Button();
            this.HumanNameLBL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PlayerNameTB
            // 
            this.PlayerNameTB.Location = new System.Drawing.Point(21, 78);
            this.PlayerNameTB.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.PlayerNameTB.Name = "PlayerNameTB";
            this.PlayerNameTB.Size = new System.Drawing.Size(369, 36);
            this.PlayerNameTB.TabIndex = 1;
            // 
            // OkBTN
            // 
            this.OkBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OkBTN.ForeColor = System.Drawing.Color.Black;
            this.OkBTN.Location = new System.Drawing.Point(261, 141);
            this.OkBTN.Name = "OkBTN";
            this.OkBTN.Size = new System.Drawing.Size(129, 39);
            this.OkBTN.TabIndex = 3;
            this.OkBTN.Text = "&OK";
            this.OkBTN.UseVisualStyleBackColor = true;
            this.OkBTN.Click += new System.EventHandler(this.OkBTN_Click);
            // 
            // CancelBTN
            // 
            this.CancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBTN.ForeColor = System.Drawing.Color.Black;
            this.CancelBTN.Location = new System.Drawing.Point(21, 141);
            this.CancelBTN.Name = "CancelBTN";
            this.CancelBTN.Size = new System.Drawing.Size(129, 39);
            this.CancelBTN.TabIndex = 2;
            this.CancelBTN.Text = "&Cancel";
            this.CancelBTN.UseVisualStyleBackColor = true;
            this.CancelBTN.Click += new System.EventHandler(this.CancelBTN_Click);
            // 
            // HumanNameLBL
            // 
            this.HumanNameLBL.AutoSize = true;
            this.HumanNameLBL.Location = new System.Drawing.Point(21, 37);
            this.HumanNameLBL.Name = "HumanNameLBL";
            this.HumanNameLBL.Size = new System.Drawing.Size(266, 29);
            this.HumanNameLBL.TabIndex = 0;
            this.HumanNameLBL.Text = "Human Player Name:";
            // 
            // PlayerName
            // 
            this.AcceptButton = this.OkBTN;
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.CancelButton = this.CancelBTN;
            this.ClientSize = new System.Drawing.Size(413, 201);
            this.Controls.Add(this.HumanNameLBL);
            this.Controls.Add(this.CancelBTN);
            this.Controls.Add(this.OkBTN);
            this.Controls.Add(this.PlayerNameTB);
            this.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "PlayerName";
            this.Text = "Edit Human Player Name";
            this.Load += new System.EventHandler(this.PlayerName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PlayerNameTB;
        private System.Windows.Forms.Button OkBTN;
        private System.Windows.Forms.Button CancelBTN;
        private System.Windows.Forms.Label HumanNameLBL;
    }
}