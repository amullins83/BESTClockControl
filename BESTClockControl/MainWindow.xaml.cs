namespace BESTClockControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The client for UDP control
        /// </summary>
        private UdpClient udpClient = new UdpClient();

        /// <summary>
        /// Intializes a new instance of the <see cref="MainWindow"/> class
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            udpClient.EnableBroadcast = true;
            udpClient.ExclusiveAddressUse = false;
        }

        /// <summary>
        /// Start the countdown
        /// </summary>
        /// <param name="sender">The button that raised the event (ignored)</param>
        /// <param name="e">The event arguments (ignored)</param>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            this.SendMessage("START");
        }

        /// <summary>
        /// Reset the countdown
        /// </summary>
        /// <param name="sender">The button that raised the event (ignored)</param>
        /// <param name="e">The event arguments (ignored)</param>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            this.SendMessage("STOP");
        }

        private void SendMessage(string message)
        {
            var bytes = Encoding.ASCII.GetBytes(message);
            udpClient.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.None, 32260));
        }
    }
}
