using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.UserPanel.Connection;

namespace FlightSimulatorApp.UserPanel.Connection
{
    /// <summary>
    /// Interface for the connection panel's VM. 
    /// this VM should hold the ip and port and send connect and disconnect commands to the model.
    /// </summary>
    public interface IConnectionPanelVM
    {
        /// <summary>
        /// the Connection panel's ip textbox content
        /// </summary>
        string Connection_IP { get; set; }
        /// <summary>
        /// the Connection panel's port textbox content
        /// </summary>
        string Connection_Port { get; set; }
        /// <summary>
        /// send the model a command to connect
        /// </summary>
        void ConnectAndStart();
        /// <summary>
        /// send the model a command to disconnect
        /// </summary>
        void Disconnect();
        /// <summary>
        /// use this function before the connect function. verify that the user entered valid IP and Port values.
        /// </summary>
        /// <returns></returns>
        bool IsValidInput();
        /// <summary>
        /// when the input is invalid, use this function to notify the model about an error message, which will forward it to the Errors panel
        /// </summary>
        void NotifyBadInput();
    }
}
