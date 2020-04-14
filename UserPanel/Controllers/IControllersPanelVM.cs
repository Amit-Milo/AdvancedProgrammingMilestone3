using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.UserPanel.Controllers {
    /// <summary>
    /// This is an interface for the controllers panel VM. 
    /// This interface holds the 4 properties that the controllers panel is responsible for,
    /// and should be able to send the simulator a command to change one of the properties' values.
    /// </summary>
    public interface IControllersPanelVM {
        double Throttle { set; }
        double Elevator { set; }
        double Rudder { set; }
        double Aileron { set; }
        /// <summary>
        /// call this function in the properties' setters to allow polymorphism.
        /// </summary>
        /// <param name="varPath"> the name of the var to change </param>
        /// <param name="varValue"> the new value of the var </param>
        void HandleFGVarSet(string varPath, double varValue);
        /// <summary>
        /// send the simulator a message to set the input var to the input value
        /// </summary>
        /// <param name="varPath"> the name of the var to change </param>
        /// <param name="varValue"> the new value of the var </param>
        void SetFGVarValue(string varPath, double varValue);
        
    }
}
