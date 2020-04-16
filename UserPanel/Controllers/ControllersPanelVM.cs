using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.UserPanel.Controllers
{
    /// <summary>
    /// the father class for the controllers panel VM. 
    /// this VM sends the simulator a command each time it receives a change, which may be problematic...
    /// </summary>
    public class ControllersPanelVM : IControllersPanelVM
    {
        private IFlightGearCommunicator model;
        private const int numberOfDigsToShow = 5;

        private double rudder; //saved for binding to the screen and showing the value
        private double elevator; //saved for binding to the screen and showing the value
        //joystick props
        public double Elevator
        {
            get
            {
                return this.elevator;
            }
            set
            {
                this.elevator = Math.Round(value,numberOfDigsToShow); //save only numberOfDigsToShow digits after the floating point
                HandleFGVarSet(properties["elevator"],value); //call the polymorphic function that handles the vars setting
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
                this.rudder = Math.Round(value,numberOfDigsToShow); //save only numberOfDigsToShow digits after the floating point
                HandleFGVarSet(properties["rudder"],value); //call the polymorphic function that handles the vars setting
            }
        }
        //sliders props
        public double Throttle
        {
            //do not even save a value. when set, just call a setter function that forwards the message. 
            set
            {
                HandleFGVarSet(properties["throttle"],value); //call the polymorphic function that handles the vars setting
            }
        }
        public double Aileron
        {
            //do not even save a value. when set, just call a setter function that forwards the message. 
            set
            {
                HandleFGVarSet(properties["aileron"],value); //call the polymorphic function that handles the vars setting
            }
        }

        // A dictionary mapping each property name to the full variable name as saved at the server.
        protected static readonly Dictionary<string,string> properties = new Dictionary<string,string>
        {
            {"throttle", "/controls/engines/current-engine/throttle" },
            {"elevator", "/controls/flight/elevator"},
            {"rudder", "/controls/flight/rudder" },
            {"aileron", "/controls/flight/aileron"}
        };

        /// <summary>
        /// the constructor.
        /// just save the input model as this's model.
        /// do not add the properties' paths, since they are not receivable.
        /// </summary>
        /// <param name="model"> the VM's model </param>
        public ControllersPanelVM(IFlightGearCommunicator model)
        {
            this.model = model;
        }


        /// <summary>
        /// in this class, just call the SetFGVar function. no complicated mechanics.
        /// </summary>
        /// <param name="varPath"> the name of the var to change </param>
        /// <param name="varValue"> the new value of the var </param>
        public virtual void HandleFGVarSet(string varPath,double varValue)
        {
            SetFGVarValue(varPath,varValue);
        }

        public void SetFGVarValue(string varPath,double varValue)
        {
            model.SetVarValue(varPath,varValue);
        }

    }
}