using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace CPM_WIN
{
    /// <summary>
    /// Manages game timer functionality with periodic speak scheduling and auto-start capabilities.
    /// </summary>
    public class TimerManager : IDisposable
    {
        private const int UI_TIMER_INTERVAL_MS = 200;
        private const int AUTO_START_DELAY_MS = 60000; // 1 minute
        public const int AUTO_START_DELAY_MINUTES = AUTO_START_DELAY_MS / 60000;
        private const int DEFAULT_SPEAK_INTERVAL_MINUTES = 10;
        private const int MIN_SPEAK_INTERVAL_MINUTES = 1;

        private readonly Timer _uiTimer;
        private readonly Timer _autoStartTimer;
        private readonly Stopwatch _gameTimer = new Stopwatch();
        private TimeSpan _nextSpeakTime = TimeSpan.FromMinutes(DEFAULT_SPEAK_INTERVAL_MINUTES);
        private TimeSpan _speakInterval = TimeSpan.FromMinutes(DEFAULT_SPEAK_INTERVAL_MINUTES);
        private bool _disposed;

        /// <summary>
        /// Occurs on each UI timer tick with the elapsed time.
        /// </summary>
        public event EventHandler<TimeSpan> OnTimerTick;

        /// <summary>
        /// Occurs when it's time to speak the elapsed time.
        /// </summary>
        public event EventHandler<TimeSpan> OnSpeakScheduled;

        /// <summary>
        /// Occurs when the auto-start timer completes.
        /// </summary>
        public event EventHandler OnAutoStartTick;

        /// <summary>
        /// Gets a value indicating whether the game timer is currently running.
        /// </summary>
        public bool IsRunning => _gameTimer.IsRunning;

        /// <summary>
        /// Gets the current elapsed time on the game timer.
        /// </summary>
        public TimeSpan Elapsed => _gameTimer.Elapsed;

        /// <summary>
        /// Initializes a new instance of the TimerManager class.
        /// </summary>
        public TimerManager()
        {
            _uiTimer = new Timer { Interval = UI_TIMER_INTERVAL_MS };
            _uiTimer.Tick += UiTimer_Tick;
            _uiTimer.Start();

            _autoStartTimer = new Timer { Interval = AUTO_START_DELAY_MS };
            _autoStartTimer.Tick += AutoStartTimer_Tick;

            _disposed = false;
        }

        /// <summary>
        /// Handles the UI timer tick event.
        /// </summary>
        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (_disposed)
                return;

            var elapsed = _gameTimer.Elapsed;
            OnTimerTick?.Invoke(this, elapsed);

            if (_gameTimer.IsRunning)
            {
                TryScheduleSpeak(elapsed);
            }
        }

        /// <summary>
        /// Handles the auto-start timer tick event.
        /// </summary>
        private void AutoStartTimer_Tick(object sender, EventArgs e)
        {
            _autoStartTimer.Stop();
            OnAutoStartTick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Attempts to schedule a speak event if enough time has elapsed.
        /// </summary>
        private void TryScheduleSpeak(TimeSpan elapsed)
        {
            if (elapsed < _nextSpeakTime)
                return;

            int intervalMinutes = Math.Max(MIN_SPEAK_INTERVAL_MINUTES, (int)_speakInterval.TotalMinutes);
            int multiplier = (int)elapsed.TotalMinutes / intervalMinutes;
            var computedNext = TimeSpan.FromMinutes((multiplier + 1) * intervalMinutes);

            OnSpeakScheduled?.Invoke(this, elapsed);
            _nextSpeakTime = computedNext;
        }

        /// <summary>
        /// Starts the game timer.
        /// </summary>
        public void Start()
        {
            if (!_disposed)
            {
                _gameTimer.Start();
            }
        }

        /// <summary>
        /// Stops the game timer.
        /// </summary>
        public void Stop()
        {
            if (!_disposed)
            {
                _gameTimer.Stop();
            }
        }

        /// <summary>
        /// Resets the game timer to zero.
        /// </summary>
        public void Reset()
        {
            if (!_disposed)
            {
                _gameTimer.Reset();
                _nextSpeakTime = _speakInterval;
            }
        }

        /// <summary>
        /// Starts the auto-start timer.
        /// </summary>
        public void StartAutoStartTimer()
        {
            if (!_disposed && !_autoStartTimer.Enabled)
            {
                _autoStartTimer.Start();
            }
        }

        /// <summary>
        /// Stops the auto-start timer.
        /// </summary>
        public void StopAutoStartTimer()
        {
            if (!_disposed)
            {
                _autoStartTimer.Stop();
            }
        }

        /// <summary>
        /// Gets the formatted elapsed time string.
        /// </summary>
        /// <returns>A formatted time string in the format "h:mm:ss" or "mm:ss".</returns>
        public string GetFormattedTime()
        {
            var elapsed = _gameTimer.Elapsed;
            return elapsed.TotalHours >= 1
                ? elapsed.ToString(@"h\:mm\:ss")
                : elapsed.ToString(@"mm\:ss");
        }

        /// <summary>
        /// Releases all resources used by the TimerManager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    _uiTimer?.Stop();
                    _autoStartTimer?.Stop();
                    _uiTimer?.Dispose();
                    _autoStartTimer?.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during TimerManager disposal: {ex.Message}");
                }
            }

            _disposed = true;
        }
    }
}