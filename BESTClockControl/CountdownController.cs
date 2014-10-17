using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BESTClockControl
{
    public class CountdownController : ICountdownController
    {
        /// <summary>
        /// The UDP client
        /// </summary>
        private IUdpClient udpClient;

        /// <summary>
        /// The owning thread's dispatcher
        /// </summary>
        private Dispatcher mainDispatcher = Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Background worker to listen for messages
        /// </summary>
        private BackgroundWorker readLoop;

        /// <summary>
        /// Fires when a "START" request is received
        /// </summary>
        public event EventHandler StartRequestReceived;

        /// <summary>
        /// Fires when a "STOP" request is received
        /// </summary>
        public event EventHandler StopRequestReceived;

        /// <summary>
        /// Fires when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the UDP client for network control
        /// </summary>
        public IUdpClient UdpClient
        {
            get
            {
                return this.udpClient;
            }

            set
            {
                if (this.udpClient != value)
                {
                    if (this.udpClient != null)
                    {
                        this.readLoop.CancelAsync();
                    }

                    this.udpClient = value;

                    this.readLoop = new BackgroundWorker();
                    this.readLoop.DoWork += this.ReadLoop_DoWork;
                    this.readLoop.WorkerSupportsCancellation = true;    
                    this.readLoop.RunWorkerAsync(this.udpClient);
                    this.OnPropertyChanged("UdpClient");
                }
            }
        }

        /// <summary>
        /// Continuously get messages from the UDP client
        /// </summary>
        /// <param name="sender">The background worker object (ignored)</param>
        /// <param name="e">The arguments for this event, including the UDP client to use</param>
        private async void ReadLoop_DoWork(object sender, DoWorkEventArgs e)
        {
            IUdpClient udpClient = (IUdpClient)e.Argument;

            while (!this.readLoop.CancellationPending)
            {
                var result = await udpClient.ReceiveAsync();
                var resultText = Encoding.ASCII.GetString(result.Buffer);
                if (resultText == "START")
                {
                    this.OnRequestReceived(this.StartRequestReceived);
                }
                else if (resultText == "STOP")
                {
                    this.OnRequestReceived(this.StopRequestReceived);
                }
            }
        }

        /// <summary>
        /// Fire the given event on the owning thread
        /// </summary>
        /// <param name="request">The event handler to execute</param>
        private void OnRequestReceived(EventHandler request)
        {
            if (request != null)
            {
                this.mainDispatcher.BeginInvoke(request, this, EventArgs.Empty);
            }
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
    }
}
