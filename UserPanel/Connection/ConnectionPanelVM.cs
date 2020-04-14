using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.UserPanel;
using FlightSimulatorApp.UserPanel.Connection;
using System.Configuration;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.UserPanel.Connection {

    public class ConnectionPanelVM : IConnectionPanelVM {
        /// <summary>
        /// the model that we use in the vm.
        /// </summary>
        private readonly IFlightGearCommunicator model;
        private string connection_IP;
        private string connection_Port;
        public string Connection_IP {
            get { return this.connection_IP; }
            set { this.connection_IP = value; }
        }
        public string Connection_Port {
            get { return this.connection_Port; }
            set { this.connection_Port = value; }
        }

        /// <summary>
        /// the constructor. get the default ip and port values from the App.config document.
        /// </summary>
        /// <param name="model"> the VM's model </param>
        public ConnectionPanelVM(IFlightGearCommunicator model) {
            this.connection_IP = ConfigurationManager.AppSettings.Get("DEFAULT_IP");
            this.connection_Port = ConfigurationManager.AppSettings.Get("DEFAULT_PORT");
            this.model = model;
        }

        public void Connect() {
            this.model.Connect(Connection_IP, Int32.Parse(Connection_Port));
        }

        public void Disconnect() {
            this.model.Disconnect();
        }
    }
}
