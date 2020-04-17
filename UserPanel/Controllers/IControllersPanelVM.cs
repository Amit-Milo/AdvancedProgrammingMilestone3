using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.UserPanel.Controllers
{
    /// <summary>
    /// This is an interface for the controllers panel VM. 
    /// This interface holds the 4 properties that the controllers panel is responsible for,
    /// and should be able to send the simulator a command to change one of the properties' values.
    /// </summary>
    public interface IControllersPanelVM : INotifyDisconnectionOccurred
    {

        double Elevator
        {
            get; set;
        }
        double Rudder
        {
            get; set;
        }
        double Throttle
        {
            get; set;
        }
        double Aileron
        {
            get; set;
        }
        /// <summary>
        /// Call this function in the properties' setters to allow polymorphism.
        /// </summary>
        /// <param name="varPath"> The name of the var to change. </param>
        /// <param name="varValue"> The new value of the var. </param>
        void HandleFGVarSet(string varPath, double varValue);
        /// <summary>
        /// Send the simulator a message to set the input var to the input value.
        /// </summary>
        /// <param name="varPath"> The name of the var to change. </param>
        /// <param name="varValue"> The new value of the var. </param>
        void SetFGVarValue(string varPath, double varValue);

    }
}
