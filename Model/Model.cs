using System;
using System.Collecti/ons.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace FlightSimulatorApp.Model {
    /// <summary>
    /// This class is the model in the MVVM architecture. 
    /// This class takes care of communicating with the simulator and calculating the map that should be displayed on the screen. 
    /// </summary>
    public class Model : IFlightGearCommunicator {
        /// <summary>
        /// The class that communicates with the simulator, delegate all functions to this class.
        /// </summary>
        private ITelnetClient telnetClient;
        /// <summary>
        /// This dictionary holds all pairs of (name of simulator var,info value of this simulator var)
        /// This allows generic work instead of hard-coded names, and makes the code a lot clearer and simple.
        /// </summary>
        volatile private IDictionary<string, FlightGearVar> vars;
        /// <summary>
        /// The observable function for property changes events.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// The observable function for error events.
        /// </summary>
        public event ErrorNotification ErrorOccurred;
        /// <summary>
        /// Should be false as long as the connection with the simulator is active.
        /// </summary>
        volatile bool stop = true;
        /// <summary>
        /// This mutex is used to make sure that the simulator does not get to requests at once and send wrong answers.
        /// </summary>
        private Mutex socketMutex = new Mutex();


        private Mutex stopMutex = new Mutex();

        private Mutex varsMutex = new Mutex();


        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="telnetClient"> the simulator communicator </param>
        public Model(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;

            varsMutex.WaitOne();
            vars = new Dictionary<string, FlightGearVar>();
            varsMutex.ReleaseMutex();
        }


        /// <summary>
        /// Add a receivable variable to the model dictionary.
        /// </summary>
        /// <param name="varName"> The name ofthe variable to add. </param>
        public void AddReceiveableVar (string varName, bool updateOnlyOnChange = true)
        {
            double DEFAULT_VALUE = 0;

            varsMutex.WaitOne();
            bool exists = vars.ContainsKey(varName);
            varsMutex.ReleaseMutex();

            if (!exists)
            {
                // Create a flight gear var based on the name and the default value.
                FlightGearVar var = new FlightGearVar(varName, DEFAULT_VALUE, updateOnlyOnChange);

                // Add the model as a listener to the variable.
                var.PropertyChanged +=
                    delegate (object sender, PropertyChangedEventArgs e)
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(var.VarName));
                    };

                varsMutex.WaitOne();
                vars.Add(varName, var);
                varsMutex.ReleaseMutex();
            }
        }


        /// <summary>
        /// Connect to a server.
        /// </summary>
        /// <param name="ip">The server's ip.</param>
        /// <param name="port"> The server's port. </param>
        public void Connect(string ip, int port) {
            try
            {
                socketMutex.WaitOne();
                this.telnetClient.Connect(ip, port);
            }
            catch
            {
                return;
            }
            finally
            {
                socketMutex.ReleaseMutex();
            }

            /* If there is a condition race give up, as if you don't you might not be connected to any server
             * but stop stays false.*/
            stopMutex.WaitOne(0);
            stop = false;
            stopMutex.ReleaseMutex();
        }


        /// <summary>
        /// Disconnect from the server.
        /// </summary>
        public void Disconnect() {
            stopMutex.WaitOne();
            stop = true;
            stopMutex.ReleaseMutex();

            try
            {
                /* If there is a condition race give up, as if you don't you might not be connected to any server
             * but stop stays false.*/
                stopMutex.WaitOne(0);
                this.telnetClient.Disconnect();
            }
            finally
            {
                stopMutex.ReleaseMutex();
            }
        }


        public void Start() {
            new Thread(delegate () {
                while (!stop) {
                    varsMutex.WaitOne();
                    foreach (string varName in this.vars.Keys) {
                        //update the vars dictionary to the simulator values
                        this.vars[varName].VarValue = this.GetFGVarValue(varName);
                    }
                    varsMutex.ReleaseMutex();
                    Thread.Sleep(250);
                }
            }
            ).Start();
        }



        private void SetFGVarValue(string varName, double value) {
            // TODO handle timeout error

            if (!stop)
            {
                try
                {
                    //lock the simulator to prevent other threads contacting it at the same time.
                    socketMutex.WaitOne();
                    //write the new value to the simulator
                    telnetClient.Write("set " + varName + " " + value.ToString() + "\n");

                    varsMutex.WaitOne();
                    if (this.vars.ContainsKey(varName))
                        //recieve the accepted simulator value (in case the value we sent is out of bound)
                        this.vars[varName].VarValue = HandleSimulatorReturn(varName);
                    varsMutex.ReleaseMutex();
                }
                catch
                {
                    this.Disconnect();
                }
                finally
                {
                    varsMutex.ReleaseMutex();

                    //done, release the simulator.
                    socketMutex.ReleaseMutex();
                }
            }
        }


        private double GetFGVarValue(string varName) {
            // TODO handle timeout error

            varsMutex.WaitOne();
            bool available = this.vars.ContainsKey(varName);
            varsMutex.ReleaseMutex();

            double returnVal;

            if (!stop && available)
            {
                try
                {
                    //lock the simulator to prevent other threads contacting it at the same time.
                    socketMutex.WaitOne();
                    //request the value from the simulator
                    telnetClient.Write("get " + varName + "\n");
                    //save the value that has been sent from the simulator
                    double value = HandleSimulatorReturn(varName);
                    return value;
                }
                catch
                {
                    this.Disconnect();
                    varsMutex.WaitOne();
                    returnVal = this.vars[varName].VarValue;
                    varsMutex.ReleaseMutex();
                    return returnVal;
                }
                finally
                {
                    //done, release the simulator.
                    socketMutex.ReleaseMutex();
                }
            }
            varsMutex.WaitOne();
            returnVal = this.vars[varName].VarValue;
            varsMutex.ReleaseMutex();
            return returnVal;
        }


        /// <summary>
        /// gets the value from the simulator and handles ERR option by returning the current value - last good known
        /// </summary>
        /// <param name="varName"> the var to get value of in case of ERR return value </param>
        /// <returns> value from the simulator if worked properly, otherwise the current saved value and error message </returns>
        private double HandleSimulatorReturn(string varName) {
            string returnValue = telnetClient.Read();
            if (returnValue == "ERR" || returnValue == "ERR\n") {
                ErrorOccurred?.Invoke(this, "error: simulator sent ERR value for var name: "+varName);
                // return the current value
                return this.vars[varName].VarValue;
            }
            //now check for any other error value:
            double result;
            try {
                result = Double.Parse(returnValue);
            } catch (Exception) {
                ErrorOccurred?.Invoke(this, "error: simulator sent unexpected value for var name: " + varName);
                // return the current value
                return this.vars[varName].VarValue;
            }
            //else, we got a valid double number, return it.
            return result;
        }


        /// <summary>
        /// Get the value of a variable.
        /// </summary>
        /// <param name="varName"> The variable name. </param>
        /// <returns> The value of the variable, if is in the model. </returns>
        public double GetVarValue(string varName)
        {
            double DEFAULT_VALUE = 0;
            return this.vars.ContainsKey(varName) ? this.vars[varName].VarValue : DEFAULT_VALUE;
        }



        /// <summary>
        /// Update a variable's value.
        /// </summary>
        /// <param name="varName"> The variable name. </param>
        /// <param name="value"> The new value. </param>
        public void SetVarValue(string varName, double value)
        {
            if (this.vars.ContainsKey(varName))
            {
                this.vars[varName].VarValue = value;
            }

            this.SetFGVarValue(varName, value);
        }

    }
}