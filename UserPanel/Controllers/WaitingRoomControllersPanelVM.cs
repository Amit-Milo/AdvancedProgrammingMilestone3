using FlightSimulatorApp.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FlightSimulatorApp.UserPanel.Controllers
{
    /// <summary>
    /// This class implements the waiting room mechanic that limits the number of set commands sent in a second.
    /// </summary>
    public class WaitingRoomControllersPanelVM : ControllersPanelVM, IWaitingRoomControllersPanelVM
    {
        private IDictionary<string, bool> isInWaitingRoom;
        private static int maxNumberOfVarUpdatesPerSecond = 10;
        protected static readonly IDictionary<string, double> mostRecentValues;

        /// <summary>
        /// The constructor.
        /// Set the isInWaitingRoom boolean dictionary.
        /// </summary>
        /// <param name="model"> The VM's model. </param>
        public WaitingRoomControllersPanelVM(IFlightGearCommunicator model) : base(model)
        {
            isInWaitingRoom = new ConcurrentDictionary<string, bool>();
            /* Add all full paths of the properties to the dictionary with a false boolean,
               Because they are not in waiting room yet. */
            foreach (KeyValuePair<string, string> entry in properties)
            {
                isInWaitingRoom.Add(entry.Value, false);
                mostRecentValues.Add(entry.Value, 0);
            }
        }

        /// <summary>
        /// In this derived class, call the waiting room function.
        /// </summary>
        /// <param name="varPath"> The name of the var to change. </param>
        /// <param name="varValue"> The new value of the var. </param>
        public override void HandleFGVarSet(string varPath, double varValue)
        {
            mostRecentValues[varPath] = varValue;
            this.WaitingRoom(varPath, varValue);
        }

        public void WaitingRoom(string varPath, double varValue)
        {
            // If not already in the waiting room:
            if (!isInWaitingRoom[varPath])
            {
                // Set that it is now in the waiting room:
                isInWaitingRoom[varPath] = true;
                new Thread(delegate ()
                {
                    // Send the simulator a command to update the var, and save the sent value:
                    double sentValue = varValue;
                    this.SetFGVarValue(varPath, varValue);
                    // We got the most recent value.
                    // Now sleep for some time, in which you can not send update commands for this var.
                    System.Threading.Thread.Sleep((int)(1000 / maxNumberOfVarUpdatesPerSecond));
                    /* If has changed while was in the waiting room, change it now,
                     * in case there wont be any more changes, so we set the most recent value.*/
                    if (mostRecentValues[varPath] != varValue)
                    {
                        this.SetFGVarValue(varPath, varValue);
                    }
                    // Finished the waiting time, set that this var is now no longer in the waiting room.
                    isInWaitingRoom[varPath] = false;
                }).Start();
            }
        }

    }
}
