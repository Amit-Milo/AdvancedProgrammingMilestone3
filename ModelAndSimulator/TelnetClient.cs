using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace Ex1.ModelAndSimulator {
    /// <summary>
    /// Implementation of the ITelnetClient interface.
    /// </summary>
    class TelnetClient : ITelnetClient {
        private TcpClient client = null;
        private NetworkStream stream = null;
        private StreamReader reader = null;

        public void Connect(string ip, int port) {
            client = new TcpClient(ip, port);
            stream = client.GetStream();
            reader = new StreamReader(stream);
        }

        public void Disconnect() {
            if (client == null) {
                throw new Exception("should connect first");
            }
            stream.Close();
            client.Close();
        }

        public string Read() {
            if (client == null) {
                throw new Exception("should connect first");
            }
            string readData = reader.ReadLine();
            return readData;
        }

        public void Write(string sendMessage) {
            if (client == null) {
                throw new Exception("should connect first");
            }
            if (sendMessage[sendMessage.Length - 1] != '\n') { //all messages should end with a '\n'
                sendMessage += '\n';
            }
            int byteCount = Encoding.ASCII.GetByteCount(sendMessage + 1);
            byte[] sendData = new byte[byteCount];
            sendData = Encoding.ASCII.GetBytes(sendMessage);
            stream.Write(sendData, 0, sendData.Length);
        }
    }
}
