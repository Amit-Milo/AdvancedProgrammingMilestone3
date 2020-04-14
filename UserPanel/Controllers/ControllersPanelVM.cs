using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;

//TODO remember to init the views in the App page.

namespace FlightSimulatorApp.UserPanel.Controllers {
    public class ControllersPanelVM : IControllersPanelVM {
        private IFlightGearCommunicator model;
        //joystick props
        public double Elevator {
            set {
                SetFGVarValue(properties["elevator"], value);
            }
        }
        public double Rudder {
            set {
                SetFGVarValue(properties["rudder"], value);
            }
        }
        //sliders props
        public double Throttle {
            set {
                SetFGVarValue(properties["throttle"], value);
            }
        }
        public double Aileron {
            set {
                SetFGVarValue(properties["aileron"], value);
            }
        }

        // A dictionary mapping each property name to the full variable name as saved at the server.
        protected static readonly Dictionary<string, string> properties = new Dictionary<string, string>
        {
            {"throttle", "/controls/engines/current-engine/throttle" },
            {"elevator", "/controls/flight/elevator"},
            {"rudder", "/controls/flight/rudder" },
            {"aileron", "/controls/flight/aileron"}
        };

        public ControllersPanelVM(IFlightGearCommunicator model) {
            this.model = model;
            //TODO add the vars paths to the model. i don't have this method.
        }

        

        /// <summary>
        /// in this class, just call the SetFGVar function. no complicated mechanics.
        /// </summary>
        /// <param name="varPath"> the name of the var to change </param>
        /// <param name="varValue"> the new value of the var </param>
        public virtual void HandleFGVarSet(string varPath, double varValue) {
            SetFGVarValue(varPath, varValue);
        }

        public void SetFGVarValue(string varPath, double varValue) {
            model.SetFGVarValue(varPath, varValue);
        }

        public void CalculateJoystickProperty(string varPath) {
            throw new NotImplementedException();
        }
    }
}