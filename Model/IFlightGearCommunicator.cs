using FlightSimulatorApp.UserPanel.Errors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlightSimulatorApp.Model
{
    public interface IFlightGearCommunicator : INotifyPropertyChanged, INotifyError, INotifyDisconnectionOccurred
    {
        /// <summary>
        /// Init the connection with the simulator.
        /// </summary>
        /// <param name="ip"> The ip address. </param>
        /// <param name="port"> The port to connect on. </param>
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
        /// <param name="varName"> The name of the variable to change. </param>
        /// <param name="value"> The new value of the variable. </param>
        void SetVarValue(string varName, double value);
        /// <summary>
        /// Send the simulator a message that gets the value of a variable.
        /// </summary>
        /// <param name="varName"> The name of the variable to get the value of. </param>
        /// <returns> The simulator's value of the input variable </returns>
        double GetVarValue(string varName);


        void AddReceiveableVar(string varName, bool updateOnlyOnChange = true);

        /// <summary>
        /// Notify the Errors panel about the new error that has occurred.
        /// </summary>
        /// <param name="errorMessage"> The error message to send. </param>
        /// <param name="additionalInfo"> More info if needed. </param>
        void NotifyError(ErrorMessages.errorsEnum errorMessage, string additionalInfo = "");
    }
}
