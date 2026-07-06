using System;
using System.Linq;

namespace CPM_WIN
{
    /// <summary>
    /// Manages the selection of computer players and human player placement.
    /// </summary>
    public class PlayerSelector
    {
        private const int MAX_PLAYER_SELECTION_ATTEMPTS = 100;
        private const int TOTAL_AVAILABLE_NAMES = 8;

        /// <summary>
        /// Array of available computer player names.
        /// </summary>
        private static readonly string[] AvailablePlayerNames =
        {
            "Arthur", "Gertrude", "Erwin", "Maude", "Carmen", "Isaac", "Penelope", "Ollie"
        };

        private readonly Random _rng = new Random();
        private bool[] _chosen = new bool[TOTAL_AVAILABLE_NAMES];

        /// <summary>
        /// Generates a comma-separated list of players with the human player and computer opponents.
        /// </summary>
        /// <param name="computerPlayers">The number of computer players to include.</param>
        /// <param name="humanPlayerName">The name of the human player.</param>
        /// <returns>A formatted string of player names separated by " - ".</returns>
        public string GeneratePlayerList(int computerPlayers, string humanPlayerName)
        {
            ResetPlayerSelection();
            string playerList = "";

            int totalPlayers = computerPlayers + 1;
            int humanPlayerIndex = _rng.Next(0, totalPlayers);

            for (int i = 0; i < totalPlayers; i++)
            {
                if (i == humanPlayerIndex)
                {
                    playerList += humanPlayerName + " - ";
                }
                else
                {
                    playerList += GetComputerPlayer() + " - ";
                }
            }

            return playerList.TrimEnd(' ', '-');
        }

        /// <summary>
        /// Resets the player selection state for a new game.
        /// </summary>
        private void ResetPlayerSelection()
        {
            Array.Clear(_chosen, 0, _chosen.Length);
        }

        /// <summary>
        /// Gets a randomly selected computer player name that hasn't been used yet.
        /// </summary>
        /// <returns>An available computer player name.</returns>
        private string GetComputerPlayer()
        {
            // If all players have been chosen, reset and start over
            if (AllPlayersChosen())
            {
                ResetPlayerSelection();
            }

            // Try to find an unchosen player
            string selectedPlayer = TryGetRandomUnChosenPlayer();
            if (selectedPlayer != null)
            {
                return selectedPlayer;
            }

            // Fallback: find the first available player
            selectedPlayer = GetFirstAvailablePlayer();
            if (selectedPlayer != null)
            {
                return selectedPlayer;
            }

            // Ultimate fallback (should rarely happen)
            return AvailablePlayerNames[0];
        }

        /// <summary>
        /// Checks if all available players have been selected.
        /// </summary>
        /// <returns>True if all players have been chosen; otherwise false.</returns>
        private bool AllPlayersChosen()
        {
            return _chosen.All(c => c);
        }

        /// <summary>
        /// Attempts to find a random unchosen player.
        /// </summary>
        /// <returns>A randomly selected unchosen player name, or null if all attempts fail.</returns>
        private string TryGetRandomUnChosenPlayer()
        {
            for (int attempts = 0; attempts < MAX_PLAYER_SELECTION_ATTEMPTS; attempts++)
            {
                int playerNum = _rng.Next(0, AvailablePlayerNames.Length);

                if (!_chosen[playerNum])
                {
                    _chosen[playerNum] = true;
                    return AvailablePlayerNames[playerNum];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the first available (unchosen) player.
        /// </summary>
        /// <returns>The first available player name, or null if none available.</returns>
        private string GetFirstAvailablePlayer()
        {
            for (int i = 0; i < AvailablePlayerNames.Length; i++)
            {
                if (!_chosen[i])
                {
                    _chosen[i] = true;
                    return AvailablePlayerNames[i];
                }
            }

            return null;
        }
    }
}