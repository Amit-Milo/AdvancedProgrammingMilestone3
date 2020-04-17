using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.UserPanel;
using FlightSimulatorApp.UserPanel.Connection;
using System.Configuration;

using FlightSimulatorApp.Model;
using FlightSimulatorApp.UserPanel.Errors;
using System.Net;

namespace FlightSimulatorApp.UserPanel.Connection
{

    public class ConnectionPanelVM : IConnectionPanelVM
    {
        /// <summary>
        /// The model that we use in the vm.
        /// </summary>
        private readonly IFlightGearCommunicator model;
        private string connection_IP;
        private string connection_Port;
        public string Connection_IP
        {
            get
            {
                return this.connection_IP;
            }
            set
            {
                this.connection_IP = value;
            }
        }
        public string Connection_Port
        {
            get
            {
                return this.connection_Port;
            }
            set
            {
                this.connection_Port = value;
            }
        }

        /// <summary>
        /// The constructor. get the default ip and port values from the App.config document.
        /// </summary>
        /// <param name="model"> The VM's model. </param>
        public ConnectionPanelVM(IFlightGearCommunicator model)
        {
            this.connection_IP = ConfigurationManager.AppSettings.Get("DEFAULT_IP");
            this.connection_Port = ConfigurationManager.AppSettings.Get("DEFAULT_PORT");
            this.model = model;
        }

        public void ConnectAndStart()
        {
            if (IsValidInput())
            {
                this.model.Connect(Connection_IP, Int32.Parse(Connection_Port));
                this.model.Start();
            }
            else
            {
                this.NotifyBadInput();
            }
        }

        public void Disconnect()
        {
            this.model.Disconnect();
        }

        public bool IsValidInput()
        {
            return IsValidIP() && IsValidPort();
        }

        private bool IsValidIP()
        {
            return IPAddress.TryParse(this.connection_IP, out _);
        }


        private bool IsValidPort()
        {
            return int.TryParse(this.Connection_Port, out _);
        }

        public void NotifyBadInput()
        {
            if (!IsValidPort())
            {
                if (!IsValidIP())
                {
                    model.NotifyError(ErrorMessages.errorsEnum.InvalidPortAndIP);
                }
                else
                {
                    model.NotifyError(ErrorMessages.errorsEnum.InvalidPort);
                }
            }
            else
            {
                if (!IsValidIP())
                {
                    model.NotifyError(ErrorMessages.errorsEnum.InavlidIP);
                }
                else
                {
                    model.NotifyError(ErrorMessages.errorsEnum.Other, "got to NotifyBadInput but both IP and port are valid");
                }
            }
        }
    }
}

