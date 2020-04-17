using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;


namespace FlightSimulatorApp.UserPanel.Controllers
{
    /// <summary>
    /// The father class for the controllers panel VM. 
    /// This VM sends the simulator a command each time it receives a change, which may be problematic...
    /// </summary>
    public class ControllersPanelVM : IControllersPanelVM
    {
        private IFlightGearCommunicator model;
        private const int numberOfDigsToShow = 5;

        // Saved for binding to the screen and showing the value:
        private double rudder;
        private double elevator;
        // Saved to be able to set to 0 when server disconnects.
        private double throttle;
        private double aileron;
        // Joystick props.
        public double Elevator
        {
            get
            {
                return this.elevator;
            }
            set
            {
                // save only numberOfDigsToShow digits after the floating point.
                this.elevator = Math.Round(value, numberOfDigsToShow);
                // Call the polymorphic function that handles the vars setting.
                HandleFGVarSet(properties["elevator"], value);
            }
        }
        public double Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                // Save only numberOfDigsToShow digits after the floating point.
                this.rudder = Math.Round(value, numberOfDigsToShow);
                // Call the polymorphic function that handles the vars setting.
                HandleFGVarSet(properties["rudder"], value);
            }
        }
        // Sliders props
        public double Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                this.throttle = value;
                // Call the polymorphic function that handles the vars setting.
                HandleFGVarSet(properties["throttle"], value);
            }
        }
        public double Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                // Call the polymorphic function that handles the vars setting.
                HandleFGVarSet(properties["aileron"], value);
            }
        }

        protected static readonly string throttlePath = "/controls/engines/current-engine/throttle";
        protected static readonly string elevatorPath = "/controls/flight/elevator";
        protected static readonly string rudderPath = "/controls/flight/rudder";
        protected static readonly string aileronPath = "/controls/flight/aileron";

        // A dictionary mapping each property name to the full variable name as saved at the server.
        protected static readonly Dictionary<string, string> properties = new Dictionary<string, string>
        {
            {"throttle", throttlePath },
            {"elevator", elevatorPath },
            {"rudder", rudderPath },
            {"aileron", aileronPath }
        };

        public event DisconnectionNotification DisconnectionOccurred;


        /// <summary>
        /// The constructor.
        /// Just save the input model as this's model.
        /// Do not add the properties' paths, since they are not receivable.
        /// Add the model a delegate function that handles disconnection.
        /// </summary>
        /// <param name="model"> The VM's model. </param>
        public ControllersPanelVM(IFlightGearCommunicator model)
        {
            this.model = model;
            this.model.DisconnectionOccurred +=
                delegate ()
                {
                    DisconnectionOccurred?.Invoke();
                };
        }


        /// <summary>
        /// In this class, just call the SetFGVar function. no complicated mechanics.
        /// </summary>
        /// <param name="varPath"> The name of the var to change. </param>
        /// <param name="varValue"> The new value of the var. </param>
        public virtual void HandleFGVarSet(string varPath, double varValue)
        {
            SetFGVarValue(varPath, varValue);
        }

        public void SetFGVarValue(string varPath, double varValue)
        {
            model.SetVarValue(varPath, varValue);
        }
    }
}