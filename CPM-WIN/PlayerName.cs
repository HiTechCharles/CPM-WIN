using System;
using System.Windows.Forms;

namespace CPM_WIN
{
    /// <summary>
    /// Dialog form for editing the human player's name.
    /// </summary>
    public partial class PlayerName : Form
    {
        private const string EMPTY_NAME_ERROR = "Please enter a player name.";
        private const string EMPTY_NAME_TITLE = "Name Required";

        /// <summary>
        /// Initializes a new instance of the PlayerName dialog form.
        /// </summary>
        public PlayerName()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the OK button click event.
        /// Validates the player name input and saves it.
        /// </summary>
        private void OkBTN_Click(object sender, EventArgs e)
        {
            string playerName = PlayerNameTB.Text;

            if (!IsValidPlayerName(playerName))
            {
                MessageBox.Show(EMPTY_NAME_ERROR, EMPTY_NAME_TITLE,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PlayerNameTB.Focus();
                return;
            }

            Form1.SetHumanPlayerName(playerName.Trim());
            Close();
        }

        /// <summary>
        /// Handles the Cancel button click event.
        /// </summary>
        private void CancelBTN_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the form load event.
        /// Populates the name field with the current player name.
        /// </summary>
        private void PlayerName_Load(object sender, EventArgs e)
        {
            PlayerNameTB.Text = Form1.HumanPlayerName;
        }

        /// <summary>
        /// Validates whether the provided player name is acceptable.
        /// </summary>
        /// <param name="name">The player name to validate.</param>
        /// <returns>True if the name is valid (non-empty/whitespace); otherwise false.</returns>
        private static bool IsValidPlayerName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }
    }
}
