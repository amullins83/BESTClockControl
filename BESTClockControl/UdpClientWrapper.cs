namespace BESTClockControl
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Wrapper around the <see cref="System.Net.UdpClient"/> class to explicitly implement our <see cref="IUdpClient"/> interface
    /// </summary>
    public class UdpClientWrapper : IUdpClient, IDisposable
    {
        /// <summary>
        /// The actual UDP client being wrapped
        /// </summary>
        private UdpClient client = new UdpClient();

        /// <summary>
        /// A value indicating whether the client has been closed
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Synchronously listen for a message (blocks the calling thread until a message is received)
        /// </summary>
        /// <param name="endpoint">The IP endpoint on which to listen</param>
        /// <returns>A byte array containing the received message</returns>
        public byte[] Receive(ref IPEndPoint endpoint)
        {
            byte[] rec = null;
            try
            {
                rec = this.client.Receive(ref endpoint);
            }
            catch (SocketException)
            {
                // Suppress error messages
            }

            return rec;
        }

        /// <summary>
        /// Asynchronously listen for a message
        /// </summary>
        /// <returns>An await-able promise object that will yield a receive result upon completion</returns>
        public async Task<UdpReceiveResult> ReceiveAsync()
        {
            return await this.client.ReceiveAsync();
        }

        /// <summary>
        /// Synchronously send the given message
        /// </summary>
        /// <param name="message">The byte array to send</param>
        /// <param name="length">The length in bytes of the message</param>
        public void Send(byte[] message, int length)
        {
            this.client.Send(message, length);
        }

        /// <summary>
        /// Asynchronously send the given message
        /// </summary>
        /// <param name="message">The byte array to send</param>
        /// <param name="length">The length in bytes of the message</param>
        /// <returns>An await-able promise object</returns>
        public async Task SendAsync(byte[] message, int length)
        {
            await this.client.SendAsync(message, length);
        }

        /// <summary>
        /// Connect to the specified address and port
        /// </summary>
        /// <param name="address">The IP address of the connecting device</param>
        /// <param name="port">The port for the connection</param>
        public void Connect(IPAddress address, int port)
        {
            this.client.Connect(address, port);
        }

        /// <summary>
        /// Close the UDP client
        /// </summary>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.Dispose(true);
                this.isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Close the UDP client
        /// </summary>
        /// <param name="isDisposing">A value indicating whether the dispose method was called directly</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing || !this.isDisposed)
            {
                this.client.Close();
            }
        }
    }
}
