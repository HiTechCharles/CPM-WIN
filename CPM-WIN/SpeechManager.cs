using System;
using System.Diagnostics;
using System.Speech.Synthesis;

namespace CPM_WIN
{
    /// <summary>
    /// Manages text-to-speech synthesis with thread-safe operations.
    /// </summary>
    public class SpeechManager : IDisposable
    {
        private const int DEFAULT_SPEECH_RATE = 3;
        private const int DEFAULT_SPEECH_VOLUME = 100;
        private const string ELAPSED_TIME_PREFIX = "Elapsed time: ";

        private readonly SpeechSynthesizer _synth;
        private readonly object _synthLock = new object();
        private bool _disposed;

        /// <summary>
        /// Gets the current state of the speech synthesizer.
        /// </summary>
        public SynthesizerState State
        {
            get
            {
                lock (_synthLock)
                {
                    return _synth?.State ?? SynthesizerState.Ready;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the SpeechManager class.
        /// </summary>
        public SpeechManager()
        {
            _synth = new SpeechSynthesizer();
            _disposed = false;

            try
            {
                _synth.Rate = DEFAULT_SPEECH_RATE;
                _synth.Volume = DEFAULT_SPEECH_VOLUME;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Speech initialization failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously speaks the provided text.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        public void SpeakAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            lock (_synthLock)
            {
                if (_disposed || _synth == null)
                    return;

                try
                {
                    _synth.SpeakAsync(text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SpeakAsync failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Synchronously speaks the provided text.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        public void Speak(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            lock (_synthLock)
            {
                if (_disposed || _synth == null)
                    return;

                try
                {
                    _synth.Speak(text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Speak failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Speaks the elapsed time in a human-readable format.
        /// </summary>
        /// <param name="ts">The elapsed time timespan.</param>
        public void SpeakElapsedTime(TimeSpan ts)
        {
            int hours = (int)ts.TotalHours;
            int minutes = ts.Minutes;
            int seconds = ts.Seconds;

            string phrase = BuildElapsedTimePhrase(hours, minutes, seconds);
            SpeakAsync(phrase);
        }

        /// <summary>
        /// Cancels all pending speech operations.
        /// </summary>
        public void CancelAll()
        {
            lock (_synthLock)
            {
                if (_disposed || _synth == null)
                    return;

                try
                {
                    _synth.SpeakAsyncCancelAll();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CancelAll failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Builds a human-readable phrase for the elapsed time.
        /// </summary>
        private static string BuildElapsedTimePhrase(int hours, int minutes, int seconds)
        {
            if (hours > 0)
            {
                return $"{ELAPSED_TIME_PREFIX}{hours} hour{Pluralize(hours)} {minutes} minute{Pluralize(minutes)}.";
            }
            else if (minutes > 0)
            {
                return $"{ELAPSED_TIME_PREFIX}{minutes} minute{Pluralize(minutes)} {seconds} second{Pluralize(seconds)}.";
            }
            else
            {
                return $"{ELAPSED_TIME_PREFIX}{seconds} second{Pluralize(seconds)}.";
            }
        }

        /// <summary>
        /// Returns the plural suffix for the given count.
        /// </summary>
        private static string Pluralize(int count)
        {
            return count != 1 ? "s" : "";
        }

        /// <summary>
        /// Releases all resources used by the SpeechManager.
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
                lock (_synthLock)
                {
                    try
                    {
                        if (_synth != null)
                        {
                            _synth.SpeakAsyncCancelAll();
                            _synth.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed disposing synth: {ex.Message}");
                    }
                }
            }

            _disposed = true;
        }
    }
}