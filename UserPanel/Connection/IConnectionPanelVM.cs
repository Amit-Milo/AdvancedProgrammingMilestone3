using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.UserPanel.Connection;

namespace FlightSimulatorApp.UserPanel.Connection {
    /// <summary>
    /// Interface for the connection panel's VM. 
    /// this VM should hold the ip and port and send connect and disconnect commands to the model.
    /// </summary>
    public interface IConnectionPanelVM {
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
        void Connect();
        /// <summary>
        /// send the model a command to disconnect
        /// </summary>
        void Disconnect();
    }
}
