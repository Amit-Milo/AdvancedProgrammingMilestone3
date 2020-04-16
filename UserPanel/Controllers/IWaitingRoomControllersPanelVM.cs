using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlightSimulatorApp.UserPanel.Controllers
{
    /// <summary>
    /// this interface is derived from the controllers panel VM interface, 
    /// and it adds another method that will be used in controllers panel VM's subclasses that implement this interface.
    /// </summary>
    public interface IWaitingRoomControllersPanelVM : IControllersPanelVM
    {
        /// <summary>
        /// put the vars on waiting room to prevent bombarding the simulator with requests. 
        /// </summary>
        /// <param name="varPath"> the name of the var to change </param>
        /// <param name="varValue"> the new value of the var </param>
        void WaitingRoom(string varPath,double varValue);
    }
}
