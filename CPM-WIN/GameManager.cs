using System;

namespace CPM_WIN
{
    /// <summary>
    /// Manages game state, levels, and rule selection for the CPM game.
    /// </summary>
    public class GameManager
    {
        private const int MAX_LEVEL = 7;
        private const int MIN_LEVEL = 1;
        private const int INITIAL_PLAYERS_OFFSET = 1;

        /// <summary>
        /// Array of game rules that can be randomly selected during gameplay.
        /// </summary>
        private static readonly string[] GameRules =
        {
            "No trades until computer offers a trade first.",
            "For your first monopoly, you must build from scratch to hotels in one turn.",
            "Always roll to get out of jail.",
            "Always pay to get out of jail.",
            "Cannot buy any railroads.",
            "Cannot buy orange or red properties.",
            "Can only build on 1 monopoly.",
            "After getting a monopoly, you must pass go 5 times before building on it.",
            "Play one of the built-in scenarios from the game editor menu.  (1 to 4 players)",
            "All Players start with $500.",
            "All players start with $2500.",
            "Must keep at least $500 available at all times.",
            "You may use the rewind feature once during your game.",
            "Play a short game.",
            "Begin a normal game, and use a 60 minute timer.",
            "Auction off the first unowned property you land on."
        };

        private readonly Random _rng = new Random();

        /// <summary>
        /// Gets the current game level.
        /// </summary>
        public int CurrentLevel { get; private set; } = MIN_LEVEL;

        /// <summary>
        /// Gets a value indicating whether a new game has been started.
        /// </summary>
        public bool NewGameStarted { get; private set; }

        /// <summary>
        /// Sets the game level if it's within the valid range.
        /// </summary>
        /// <param name="level">The level to set (must be between MIN_LEVEL and MAX_LEVEL).</param>
        public void SetLevel(int level)
        {
            if (level >= MIN_LEVEL && level <= MAX_LEVEL)
            {
                CurrentLevel = level;
            }
        }

        /// <summary>
        /// Gets a random game rule.
        /// </summary>
        /// <returns>A randomly selected game rule string.</returns>
        public string GetRandomGameRule()
        {
            int ruleIndex = _rng.Next(0, GameRules.Length);
            return GameRules[ruleIndex];
        }

        /// <summary>
        /// Marks the start of a new game.
        /// </summary>
        public void StartNewGame()
        {
            NewGameStarted = true;
        }

        /// <summary>
        /// Resets the game state.
        /// </summary>
        public void ResetGameState()
        {
            NewGameStarted = false;
        }

        /// <summary>
        /// Gets a formatted string showing the number of computer and total players.
        /// </summary>
        /// <returns>A string in the format "{CurrentLevel} Computer, {TotalPlayers} Total".</returns>
        public string GetNumPlayersText()
        {
            int totalPlayers = CurrentLevel + INITIAL_PLAYERS_OFFSET;
            return $"{CurrentLevel} Computer, {totalPlayers} Total";
        }
    }
}