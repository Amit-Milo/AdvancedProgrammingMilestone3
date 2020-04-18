
using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    /// <summary>
    /// Implementation of the ITelnetClient interface.
    /// </summary>
    public class TelnetClient : ITelnetClient
    {
        private TcpClient client = null;
        private NetworkStream stream = null;
        private StreamReader reader = null;

        private Mutex mutex = new Mutex();

        private Stopwatch stopWatch = new Stopwatch();

        private int timesOutLately = 0;


        /// <summary>
        /// Connect to a specifiec host.
        /// </summary>
        /// <param name="ip"> The host's ip. </param>
        /// <param name="port"> The hosts port. </param>
        public void Connect(string ip, int port)
        {
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
        public void Disconnect()
        {
            if (client == null)
            {
                throw new Exception("should connect first");
            }
            stream.Close();
            reader.Close();
            client.Close();

            client = null;
            timesOutLately = 0;
        }


        /// <summary>
        /// Try read data sent by the host.
        /// </summary>//
        /// <returns> The data sent by the host, if the host indeed sent something </returns>
        public string Read()
        {
            // If client isn't connected there should be no read action.
            if (client == null)
            {
                throw new Exception("should connect first");
            }
            if (!client.Connected)
                throw new ServerNotConnectedException();

            // Read data from server, in a threading safe way.
            mutex.WaitOne();
            try
            {
                string readData = null;
                timesOutLately++;

                // Handle all timeouts.
                while (timesOutLately > 0)
                {
                    // Try to read from host.
                    stopWatch.Start();
                    readData = reader.ReadLine();
                    stopWatch.Stop();
                    stopWatch.Reset();

                    timesOutLately--;
                }
                return readData;
            }
            catch (IOException)
            {
                // Stop delay count and create appropriate exception.
                stopWatch.Stop();
                IOException iOException = new IOException(String.Format("Server didn't respond after {0} seconds.",
                    stopWatch.ElapsedMilliseconds / 1000));

                // Reset delay count.
                stopWatch.Reset();

                // Throw the exception.
                throw iOException;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }


        /// <summary>
        /// Send a message to the host.
        /// </summary>
        /// <param name="sendMessage"> The message to send. </param>
        public void Write(string sendMessage)
        {
            if (client == null)
            {
                throw new Exception("should connect first");
            }
            if (!client.Connected)
                throw new ServerNotConnectedException();

            // Add a '\n' to the end of the message if there is no \n there. 
            if (sendMessage[sendMessage.Length - 1] != '\n')
            { // All messages should end with a '\n'.
                sendMessage += '\n';
            }

            // Encode the message.
            int byteCount = Encoding.ASCII.GetByteCount(sendMessage + 1);
            byte[] sendData = new byte[byteCount];
            sendData = Encoding.ASCII.GetBytes(sendMessage);

            // Send the message in a threading safe way.
            mutex.WaitOne();
            try
            {
                // Try to write to the host.
                stopWatch.Start();
                stream.Write(sendData, 0, sendData.Length);
                stopWatch.Stop();
                stopWatch.Reset();
            }
            catch (IOException)
            {
                // Measure delay time and create appropriate exception.
                stopWatch.Stop();
                IOException iOException = new IOException(String.Format("Server didn't respond after {0} seconds.",
                    stopWatch.ElapsedMilliseconds / 1000));

                // Reset the total delay time.
                stopWatch.Reset();

                // Increase timeout cases.
                timesOutLately++;

                // Throw the above exception.
                throw iOException;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
