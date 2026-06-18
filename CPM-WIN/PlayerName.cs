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
            Form1.HumanPlayerName = PlayerNameTB.Text;
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
