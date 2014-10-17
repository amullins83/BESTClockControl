namespace BESTClockControl
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Interface for Countdown view models
    /// </summary>
    public interface ICountdownViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Fires when the countdown is over
        /// </summary>
        event EventHandler TimedOut;

        /// <summary>
        /// Gets the string representation of the remaining time
        /// </summary>
        string TimeRemaining { get; }

        /// <summary>
        /// Gets or sets the initial countdown time
        /// </summary>
        TimeSpan StartTime { get; set; }

        /// <summary>
        /// Starts the countdown
        /// </summary>
        void Start();

        /// <summary>
        /// Pauses the countdown
        /// </summary>
        void Stop();

        /// <summary>
        /// Resets the counter to the start time
        /// </summary>
        void Reset();
    }
}
