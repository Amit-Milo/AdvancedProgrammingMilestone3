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
    /// this implementation takes the latest error and prints it to the screen for a certain number of seconds.
    /// </summary>
    public class ErrorsPanelVM : IErrorsPanelMessager
    {
        /// <summary>
        /// the model that we use in the vm.
        /// </summary>
        private readonly IFlightGearCommunicator model;
        /// <summary>
        /// number of seconds to show the error message on the screen
        /// </summary>
        private readonly double errorMessageShowingTimeSeconds;
        /// <summary>
        /// the time of the last error message, is used to check whether we should clear the message of not 
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
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("ErrorMessage"));
            }
        }

        /// <summary>
        /// the constructor. set the vm's model to the input model, add the error event, 
        /// and set the current error to empty.
        /// </summary>
        /// <param name="model"> the VM's model </param>
        public ErrorsPanelVM(IFlightGearCommunicator model,double errorMessageShowingTimeSeconds)
        {
            this.model = model;
            model.ErrorOccurred +=
                delegate (object sender,string error)
                {
                    this.HandleError(error);
                };
            ErrorMessage = "";
            this.errorMessageShowingTimeSeconds = errorMessageShowingTimeSeconds;
        }

        /// <summary>
        /// default constructor with 10 seconds error displaying time. 
        /// </summary>
        /// <param name="model"> the VM's model </param>
        public ErrorsPanelVM(IFlightGearCommunicator model) : this(model,10) { }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// set the Error Message field to the new error message.
        /// erase the input error message after errorMessageShowingTimeSeconds.
        /// to do so, start a new thread and let it sleep for this number of seconds.
        /// after that time, check if the current error message is the input one,
        /// the other case is there was another error message while we were waiting.
        /// if it is indeed the input error, erase it.
        /// </summary>
        /// <param name="error"> the error message to show </param>
        public void HandleError(string error)
        {
            DateTime currTime = DateTime.Now;
            this.ErrorMessage = error + "  " + currTime.ToLongTimeString();
            this.lastErrorTime = currTime;
            new Thread(delegate ()
            {
                System.Threading.Thread.Sleep((int)(1000 * errorMessageShowingTimeSeconds));
                //if the last error message is this one:
                if (this.lastErrorTime == currTime)
                {
                    ErrorMessage = "";
                }
            }).Start();
        }
    }
}
