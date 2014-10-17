namespace BESTClockControl
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for Countdown controllers
    /// </summary>
    public interface ICountdownController : INotifyPropertyChanged
    {
        /// <summary>
        /// Fires when a "START" request is received
        /// </summary>
        event EventHandler StartRequestReceived;

        /// <summary>
        /// Fires when a "STOP" request is received
        /// </summary>
        event EventHandler StopRequestReceived;

        /// <summary>
        /// Gets or sets the UDP client for network control
        /// </summary>
        IUdpClient UdpClient { get; set; }
    }
}
