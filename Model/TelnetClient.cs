using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model {
    /// <summary>
    /// Implementation of the ITelnetClient interface.
    /// </summary>
    public class TelnetClient : ITelnetClient {
        private TcpClient client = null;
        private NetworkStream stream = null;
        private StreamReader reader = null;

        private Mutex mutex = new Mutex();


        /// <summary>
        /// Connect to a specifiec host.
        /// </summary>
        /// <param name="ip"> The host's ip </param>
        /// <param name="port"> The hosts port </param>
        public void Connect(string ip, int port) {
            // Instantiate the tcp client.
            client = new TcpClient(ip, port)
            {
                // Set time out values.
                ReceiveTimeout = 10000,
                SendTimeout = 10000
            };

            // Get the stream and the reader associated with the client.
            stream = client.GetStream();
            reader = new StreamReader(stream);
        }


        /// <summary>
        /// Disconnect the client.
        /// </summary>
        public void Disconnect() {
            if (client == null) {
                throw new Exception("should connect first");
            }
            stream.Close();
            client.Close();
        }


        /// <summary>
        /// Try read data sent by the host.
        /// </summary>
        /// <returns> The data sent by the host, if the host indeed sent something </returns>
        public string Read() {
            // If client isn't connected there should be no read action.
            if (client == null) {
                throw new Exception("should connect first");
            }

            // Read data from server, in a threading safe way.
            mutex.WaitOne();
            string readData = reader.ReadLine();
            mutex.ReleaseMutex();

            return readData;
        }


        /// <summary>
        /// Send a message to the host.
        /// </summary>
        /// <param name="sendMessage"> The message to send </param>
        public void Write(string sendMessage) {
            if (client == null) {
                throw new Exception("should connect first");
            }

            // Add a '\n' to the end of the message if there is no \n there. 
            if (sendMessage[sendMessage.Length - 1] != '\n') { //all messages should end with a '\n'
                sendMessage += '\n';
            }

            // Encode the message.
            int byteCount = Encoding.ASCII.GetByteCount(sendMessage + 1);
            byte[] sendData = new byte[byteCount];
            sendData = Encoding.ASCII.GetBytes(sendMessage);

            // Send the message in a threading safe way.
            mutex.WaitOne();
            stream.Write(sendData, 0, sendData.Length);
            mutex.ReleaseMutex();
        }
    }
}
