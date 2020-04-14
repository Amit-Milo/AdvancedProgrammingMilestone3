using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.UserPanel.Controllers {
    public interface IWaitingRoomControllersPanelVM : IControllersPanelVM {
        /// <summary>
        /// put the vars on waiting room to prevent bombarding the simulator with requests. 
        /// </summary>
        /// <param name="varPath"> the name of the var to change </param>
        /// <param name="varValue"> the new value of the var </param>
        void WaitingRoom(string varPath, double varValue);
    }
}
