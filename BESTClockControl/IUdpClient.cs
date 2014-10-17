namespace BESTClockControl
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for UDP client classes
    /// </summary>
    public interface IUdpClient
    {
        /// <summary>
        /// Synchronously listen for data on the given endpoint
        /// </summary>
        /// <param name="endpoint">The IP endpoint on which to listen</param>
        /// <returns>An array of bytes received</returns>
        byte[] Receive(ref IPEndPoint endpoint);

        /// <summary>
        /// Asynchronously listen for data
        /// </summary>
        /// <returns>An await-able promise object which yields a UDP receive result</returns>
        Task<UdpReceiveResult> ReceiveAsync();

        /// <summary>
        /// Synchronously send data
        /// </summary>
        /// <param name="message">The byte array to send</param>
        /// <param name="length">The length of the message in bytes</param>
        void Send(byte[] message, int length);

        /// <summary>
        /// Asynchronously send data
        /// </summary>
        /// <param name="message">The byte array to send</param>
        /// <param name="length">The length of the message in bytes</param>
        /// <returns>An await-able promise object</returns>
        Task SendAsync(byte[] message, int length);

        /// <summary>
        /// Connect to the given endpoint
        /// </summary>
        /// <param name="address">The IP address of the connecting device</param>
        /// <param name="port">The port for the connection</param>
        void Connect(IPAddress address, int port);
    }
}
