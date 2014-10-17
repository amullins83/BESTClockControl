namespace BESTClockControl
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// Basic implementation of the countdown view model
    /// </summary>
    public class CountdownViewModel : ICountdownViewModel
    {
        /// <summary>
        /// The actual time remaining in the countdown
        /// </summary>
        private TimeSpan timeRemaining;

        /// <summary>
        /// The starting time from which to count down
        /// </summary>
        private TimeSpan startTime;

        /// <summary>
        /// The timer to update the display
        /// </summary>
        private DispatcherTimer clockTimer;

        /// <summary>
        /// Capture a reference to the owning thread's dispatcher
        /// </summary>
        private Dispatcher mainDispatcher = Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Fires when any property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires when the countdown is over
        /// </summary>
        public event EventHandler TimedOut;

        /// <summary>
        /// Gets the string representation of the remaining time
        /// </summary>
        public string TimeRemaining
        {
            get
            {
                return this.timeRemaining.ToString(@"m\:ss");
            }
        }

        /// <summary>
        /// Gets or sets the initial countdown time
        /// </summary>
        public TimeSpan StartTime
        {
            get
            {
                return this.startTime;
            }

            set
            {
                if (this.startTime != value)
                {
                    this.startTime = value;
                    this.OnPropertyChanged("StartTime");
                }
            }
        }

        /// <summary>
        /// Starts the countdown
        /// </summary>
        public void Start()
        {
            this.clockTimer = new DispatcherTimer(
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                this.OnTick,
                this.mainDispatcher);
        }

        /// <summary>
        /// Pauses the countdown
        /// </summary>
        public void Stop()
        {
            if (this.clockTimer != null)
            {
                this.clockTimer.Stop();
            }
            this.OnPropertyChanged("TimeRemaining");
        }

        /// <summary>
        /// Resets the counter to the start time
        /// </summary>
        public void Reset()
        {
            this.timeRemaining = this.startTime;
            this.Stop();
        }

        /// <summary>
        /// Handle countdown events
        /// </summary>
        /// <param name="sender">The dispatcher timer that raised the event (ignored)</param>
        /// <param name="e">The timeout event arguments (ignored)</param>
        private void OnTick(object sender, EventArgs e)
        {
            if (this.timeRemaining <= TimeSpan.FromSeconds(1))
            {
                this.timeRemaining = TimeSpan.Zero;
                this.clockTimer.Stop();
                this.OnTimedOut();
            }
            else
            {
                this.timeRemaining -= TimeSpan.FromSeconds(1);
            }

            this.OnPropertyChanged("TimeRemaining");
        }

        /// <summary>
        /// Fire the PropertyChanged event on the owning thread
        /// </summary>
        /// <param name="name">The name of the property that changed</param>
        private void OnPropertyChanged(string name)
        {
            var propChanged = this.PropertyChanged;

            if (propChanged != null)
            {
                this.mainDispatcher.BeginInvoke(propChanged, this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Fire the TimedOut event on the owning thread
        /// </summary>
        private void OnTimedOut()
        {
            var timedOut = this.TimedOut;

            if (timedOut != null)
            {
                this.mainDispatcher.BeginInvoke(timedOut, this, EventArgs.Empty);
            }
        }
    }
}
