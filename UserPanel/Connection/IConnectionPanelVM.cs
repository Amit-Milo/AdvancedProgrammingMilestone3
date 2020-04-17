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
        /// The Connection panel's ip textbox content.
        /// </summary>
        string Connection_IP
        {
            get; set;
        }
        /// <summary>
        /// The Connection panel's port textbox content.
        /// </summary>
        string Connection_Port
        {
            get; set;
        }
        /// <summary>
        /// Send the model a command to connect.
        /// </summary>
        void ConnectAndStart();
        /// <summary>
        /// Send the model a command to disconnect.
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Use this function before the connect function. verify that the user entered valid IP and Port values.
        /// </summary>
        /// <returns> Whether the input ip and port are valid. </returns>
        bool IsValidInput();
        /// <summary>
        /// When the input is invalid, use this function to notify the model about an error message, which will forward it to the Errors panel.
        /// </summary>
        void NotifyBadInput();
    }
}
