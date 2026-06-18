using System;
using System.Windows.Forms;

namespace CPM_WIN
{
    public partial class PlayerName : Form
    {
        public PlayerName()
        {
            InitializeComponent();
        }

        private void OkBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PlayerNameTB.Text))
            {
                MessageBox.Show("Please enter a player name.", "Name Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PlayerNameTB.Focus();
                return;
            }

            Form1.HumanPlayerName = PlayerNameTB.Text.Trim();
            Close();
        }

        private void CancelBTN_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PlayerName_Load(object sender, EventArgs e)
        {
            PlayerNameTB.Text = Form1.HumanPlayerName;
        }
    }
}
