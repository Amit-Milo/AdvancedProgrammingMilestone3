using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model {
    /// <summary>
    /// This class takes care of client-server function. 
    /// Should be able to conneect and disconnect, as well as reading and writing to the server.
    /// </summary>
    public interface ITelnetClient {
        /// <summary>
        /// Connect to the server.
        /// </summary>
        /// <param name="ip"> the ip to connect </param>
        /// <param name="port"> the port to connect on </param>
        void Connect(string ip, int port);
        /// <summary>
        /// Disconnect from the server.
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Write a string to the server.
        /// </summary>
        /// <param name="command"> the string to write </param>
        void Write(string command);
        /// <summary>
        /// Read a string from the server
        /// </summary>
        /// <returns> the string that the server sent </returns>
        string Read();
    }
}
