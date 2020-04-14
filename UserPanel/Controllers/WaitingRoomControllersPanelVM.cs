﻿using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.UserPanel.Controllers {
    public class WaitingRoomControllersPanelVM : ControllersPanelVM, IWaitingRoomControllersPanelVM {
        private IDictionary<string, bool> isInWaitingRoom;
        private static int maxNumberOfVarUpdatesPerSecond = 10;

        /// <summary>
        /// the constructor.
        /// set the isInWaitingRoom boolean dictionary.
        /// </summary>
        /// <param name="model"> the VM's model </param>
        public WaitingRoomControllersPanelVM(IFlightGearCommunicator model) : base(model) {
            isInWaitingRoom = new Dictionary<string, bool>();
            //add all full paths of the properties to the dictionary with a false boolean,
            //because they are not in waiting room yet
            foreach (KeyValuePair<string, string> entry in properties) {
                isInWaitingRoom.Add(entry.Value, false);
            }
        }

        /// <summary>
        /// in this derived class, call the waiting room function.
        /// </summary>
        /// <param name="varPath"> the name of the var to change </param>
        /// <param name="varValue"> the new value of the var </param>
        public override void HandleFGVarSet(string varPath, double varValue) {
            this.WaitingRoom(varPath, varValue);
        }

        public void WaitingRoom(string varPath, double varValue) {
            //if not already in the waiting room:
            if (!isInWaitingRoom[varPath]) {
                //set that it is now in the waiting room:
                isInWaitingRoom[varPath] = true;
                new Thread(delegate () {
                    //send the simulator a command to update the var:
                    this.SetFGVarValue(varPath, varValue);
                    //now sleep for some time, in which you can not send update commands for this var.
                    System.Threading.Thread.Sleep((int)(1000 / maxNumberOfVarUpdatesPerSecond));
                    //finished the waiting time, set that this var is now no longer in the waiting room
                    isInWaitingRoom[varPath] = false;
                }).Start();
            }
        }

    }
}