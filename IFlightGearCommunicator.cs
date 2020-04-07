using FlightSimulatorApp.Notifyers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model {
    public interface IFlightGearCommunicator : INotifyPropertyChanged, INotifyError {
        /// <summary>
        /// Init the connection with the simulator.
        /// </summary>
        /// <param name="ip"> the ip address </param>
        /// <param name="port"> the port to connect on </param>
        void Connect(string ip, int port);
        /// <summary>
        /// Disconnect from the simulator.
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Start the simulator handling.
        /// </summary>
        void Start();
        /// <summary>
        /// Send the simulator a message that sets a new value to a variable.
        /// </summary>
        /// <param name="varName"> the name of the variable to change </param>
        /// <param name="value"> the new value of the variable </param>
        void SetFGVarValue(string varName, double value);
        /// <summary>
        /// Send the simulator a message that gets the value of a variable.
        /// </summary>
        /// <param name="varName"> the name of the variable to get the value of </param>
        /// <returns> the simulator's value fo the input variable </returns>
        double GetFGVarValue(string varName);
    }
}
