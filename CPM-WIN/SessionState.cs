using System;

namespace CPM_WIN
{
    /// <summary>
    /// Represents a persisted session state for the current game in progress.
    /// </summary>
    /// <summary>
    /// Serializable container representing the persisted in-progress session state.
    /// Stored to disk so a running game can be restored after an unexpected exit.
    /// </summary>
    [Serializable]
    public class SessionState
    {
        /// <summary>
        /// The Date/time when the session was created or saved.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Human-readable number of players (e.g. "1 Computer, 2 Total").
        /// </summary>
        public string NumPlayers { get; set; }

        /// <summary>
        /// Serialized player names string.
        /// </summary>
        public string Names { get; set; }

        /// <summary>
        /// The game rule text in effect for the saved session.
        /// </summary>
        public string GameRule { get; set; }

        /// <summary>
        /// Elapsed time stored as ticks to ensure compatibility with XmlSerializer.
        /// Use <see cref="Elapsed"/> for a TimeSpan view.
        /// </summary>
        public long ElapsedTicks { get; set; }

        /// <summary>
        /// The elapsed time represented as a TimeSpan. This property is ignored by the XML serializer
        /// and backed by <see cref="ElapsedTicks"/>.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public TimeSpan Elapsed
        {
            get => TimeSpan.FromTicks(ElapsedTicks);
            set => ElapsedTicks = value.Ticks;
        }

        /// <summary>
        /// Current asset total stored for the session.
        /// </summary>
        public decimal Assets { get; set; }

        /// <summary>
        /// Game difficulty/level index.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// True when the game timer was running at the time of save.
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// True when a new game had been started for the saved session.
        /// </summary>
        public bool NewGameStarted { get; set; }

        /// <summary>
        /// The human player's name saved with the session.
        /// </summary>
        public string HumanPlayerName { get; set; }
    }
}
