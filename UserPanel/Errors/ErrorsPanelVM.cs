using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FlightSimulatorApp.Model;
using System.Globalization;
using System.ComponentModel;

namespace FlightSimulatorApp.UserPanel.Errors
{
    /// <summary>
    /// An implementation of the Errors panel vm.
    /// This implementation takes the latest error and prints it to the screen for a certain number of seconds.
    /// </summary>
    public class ErrorsPanelVM : IErrorsPanelMessager
    {
        /// <summary>
        /// The model that we use in the vm.
        /// </summary>
        private readonly IFlightGearCommunicator model;
        /// <summary>
        /// Number of seconds to show the error message on the screen.
        /// </summary>
        private readonly double errorMessageShowingTimeSeconds;
        /// <summary>
        /// The time of the last error message, is used to check whether we should clear the message of not.
        /// </summary>
        private DateTime lastErrorTime;
        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
        }

        /// <summary>
        /// The constructor. set the vm's model to the input model, add the error event, 
        /// And set the current error to empty.
        /// </summary>
        /// <param name="model"> the VM's model </param>
        public ErrorsPanelVM(IFlightGearCommunicator model, double errorMessageShowingTimeSeconds)
        {
            this.model = model;
            model.ErrorOccurred +=
                delegate (object sender, string error)
                {
                    this.HandleError(error);
                };
            ErrorMessage = "";
            this.errorMessageShowingTimeSeconds = errorMessageShowingTimeSeconds;
        }

        /// <summary>
        /// Default constructor with 10 seconds error displaying time. 
        /// </summary>
        /// <param name="model"> The VM's model. </param>
        public ErrorsPanelVM(IFlightGearCommunicator model) : this(model, 10) { }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Set the Error Message field to the new error message.
        /// Erase the input error message after errorMessageShowingTimeSeconds.
        /// To do so, start a new thread and let it sleep for this number of seconds.
        /// After that time, check if the current error message is the input one,
        /// The other case is there was another error message while we were waiting.
        /// If it is indeed the input error, erase it.
        /// </summary>
        /// <param name="error"> The error message to show. </param>
        public void HandleError(string error)
        {
            DateTime currTime = DateTime.Now;
            this.ErrorMessage = error + "  " + currTime.ToLongTimeString();
            this.lastErrorTime = currTime;
            new Thread(delegate ()
            {
                System.Threading.Thread.Sleep((int)(1000 * errorMessageShowingTimeSeconds));
                // If the last error message is this one:
                if (this.lastErrorTime == currTime)
                {
                    ErrorMessage = "";
                }
            }).Start();
        }
    }
}
