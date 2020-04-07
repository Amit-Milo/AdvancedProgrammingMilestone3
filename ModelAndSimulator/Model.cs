﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Ex1.ModelAndSimulator {
    /// <summary>
    /// This class is the model in the MVVM architecture. 
    /// This class takes care of communicating with the simulator and calculating the map that should be displayed on the screen. 
    /// </summary>
    class Model : IFlightGearCommunicator {
        /// <summary>
        /// The class that communicates with the simulator, delegate all functions to this class.
        /// </summary>
        private ITelnetClient telnetClient;
        /// <summary>
        /// This dictionary holds all pairs of (name of simulator var,info value of this simulator var)
        /// This allows generic work instead of hard-coded names, and makes the code a lot clearer and simple.
        /// </summary>
        private IDictionary<string, FlightGearVar> vars;
        /// <summary>
        /// The observable function
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Should be false as long as the connection with the simuator is active.
        /// </summary>
        volatile bool stop;
        /// <summary>
        /// This mutex is used to make sure that the simulator does not get to requests at once and send wrong answers.
        /// </summary>
        private static Mutex mut = new Mutex();

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="telnetClient"> the simulator communicator </param>
        /// <param name="varsNames"> a list of all the simulator variables names, should be converted to a dictionary </param>
        public Model(ITelnetClient telnetClient, List<string> varsNames) {
            this.telnetClient = telnetClient;
            vars = CreateVars(varsNames);
        }
        /// <summary>
        /// Converts a list of simulator variables names to a dictionary
        /// </summary>
        /// <param name="varsNames"> the list of the simulator varibales names </param>
        /// <returns> a dictionary with the simulator variables names as keys, and simulator variables info obejcts as values </returns>
        private IDictionary<string, FlightGearVar> CreateVars(List<string> varsNames) {
            IDictionary<string, FlightGearVar> varSetting = new Dictionary<string, FlightGearVar>();
            foreach (string name in varsNames) { // create a new pair for the dictionary
                FlightGearVar currVar = new FlightGearVar(name, 0);
                currVar.PropertyChanged +=
                    delegate (object sender, PropertyChangedEventArgs e) {
                        this.NotifyPropertyChanged(currVar);
                        //TODO add UpdateMap ***************************************************************************************
                    };
                varSetting.Add(name, currVar);
            }
            return varSetting;
        }

        public void Connect(string ip, int port) {
            this.telnetClient.Connect(ip, port);
            stop = false;
        }

        public void Disconnect() {
            stop = true;
            this.telnetClient.Disconnect();
        }

        public void NotifyPropertyChanged(FlightGearVar flightGearVar) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(flightGearVar));
            }
        }

        public void Start() {
            new Thread(delegate () {
                while (!stop) {
                    foreach (string varName in this.vars.Keys) { //update the vars dictionary to the simulator values
                        this.vars[varName].VarValue = this.GetFGVarValue(varName);
                    }
                    Thread.Sleep(250);
                }
            }
            ).Start();
        }

        //TODO handle ERR value return using try-catch
        public void SetFGVarValue(string varName, double value) {
            mut.WaitOne(); //lock the simulator to prevent other threads contacting it at the same time.
            telnetClient.Write("set " + varName + " " + value.ToString() + "\n"); //write the new value to the simulator
            vars[varName].VarValue = Double.Parse(telnetClient.Read()); //recieve the accepted simulator value (in case the value we sent is out of bound)
            mut.ReleaseMutex(); //done, release the simulator.
        }

        public double GetFGVarValue(string varName) {
            mut.WaitOne(); //lock the simulator to prevent other threads contacting it at the same time.
            telnetClient.Write("get " + varName + "\n"); //request the value from the simulator
            double value = Double.Parse(telnetClient.Read()); //save the value that has been sent from the simulator
            mut.ReleaseMutex(); //done, release the simulator.
            return value;
        }
    }
}