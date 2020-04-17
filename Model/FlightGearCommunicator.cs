using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using FlightSimulatorApp.UserPanel.Errors;
using FlightSimulator.Model;

namespace FlightSimulatorApp.Model
{
    /// <summary>
    /// This class is the model in the MVVM architecture. 
    /// This class takes care of communicating with the simulator and calculating the map that should be displayed on the screen. 
    /// </summary>
    public class FlightGearCommunicator : IFlightGearCommunicator
    {
        /// <summary>
        /// The class that communicates with the simulator, delegate all functions to this class.
        /// </summary>
        private ITelnetClient telnetClient;
        /// <summary>
        /// This dictionary holds all pairs of (name of simulator var,info value of this simulator var).
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
        /// The observable function for disconnection events.
        /// </summary>
        public event DisconnectionNotification DisconnectionOccurred;

        /// <summary>
        /// Should be false as long as the connection with the simulator is active.
        /// </summary>
        volatile bool stop = true;
        /// <summary>
        /// This mutex is used to make sure that the simulator does not get to requests at once and send wrong answers.
        /// </summary>
        private Mutex socketMutex = new Mutex();
        /// <summary>
        /// Handle access to a thread's common resource - the stop variable.
        /// </summary>
        private Mutex stopMutex = new Mutex();

        /// <summary>
        /// Time between each simulator sampling in milliseconds.
        /// </summary>
        private static readonly int samplingRate = 250;

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="telnetClient"> The simulator communicator. </param>
        public FlightGearCommunicator(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            vars = new ConcurrentDictionary<string, FlightGearVar>();
        }


        /// <summary>
        /// Add a receivable variable to the model dictionary.
        /// </summary>
        /// <param name="varName"> The name of the variable to add. </param>
        public void AddReceiveableVar(string varName, bool updateOnlyOnChange = true)
        {
            double DEFAULT_VALUE = 0;

            if (!vars.ContainsKey(varName))
            {
                // Create a flight gear var based on the name and the default value.
                FlightGearVar var = new FlightGearVar(varName, DEFAULT_VALUE, updateOnlyOnChange);

                // Add the model as a listener to the variable.
                var.PropertyChanged +=
                    delegate (object sender, PropertyChangedEventArgs e)
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(var.VarName));
                    };

                vars.Add(varName, var);
            }
        }


        /// <summary>
        /// Connect to a server.
        /// </summary>
        /// <param name="ip"> The server's ip. </param>
        /// <param name="port"> The server's port. </param>
        public void Connect(string ip, int port)
        {
            try
            {
                this.Disconnect();
            }
            catch
            {
            }

            try
            {
                // Try to connect to host.
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
        public void Disconnect()
        {
            stopMutex.WaitOne();
            stop = true;
            stopMutex.ReleaseMutex();

            try
            {
                /* If there is a condition race give up, as if you don't you might not be connected to any server
                 * but stop stays false.*/
                stopMutex.WaitOne(0);
                this.telnetClient.Disconnect();
                // Notify about the disconnection.
                DisconnectionOccurred?.Invoke();
            }
            finally
            {
                stopMutex.ReleaseMutex();
            }
        }


        public void Start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    foreach (string varName in this.vars.Keys)
                    {
                        if (stop) break;
                        // Update the vars dictionary to the simulator values.
                        this.vars[varName].VarValue = this.GetFGVarValue(varName);
                    }
                    if (stop) break;
                    Thread.Sleep(samplingRate);
                }
            }
            ).Start();
        }


        private void SetFGVarValue(string varName, double value)
        {
            if (!stop)
            {
                try
                {
                    // Lock the simulator to prevent other threads contacting it at the same time.
                    socketMutex.WaitOne();
                    // Write the new value to the simulator.
                    telnetClient.Write("set " + varName + " " + value.ToString() + "\n");

                    if (this.vars.ContainsKey(varName))
                        // Recieve the accepted simulator value (in case the value we sent is out of bound)
                        this.vars[varName].VarValue = HandleSimulatorReturn(varName);
                    else
                        telnetClient.Read();
                }
                catch (ServerNotConnectedException)
                {
                    NotifyError(ErrorMessages.errorsEnum.ServerDisconnected);
                    this.Disconnect();
                }
                catch (Exception e)
                {
                    NotifyError(ErrorMessages.errorsEnum.Other, e.Message);
                }
                finally
                {
                    socketMutex.ReleaseMutex();
                }
            }
        }


        private double GetFGVarValue(string varName)
        {
            bool available = this.vars.ContainsKey(varName);

            double returnVal;

            if (!stop && available)
            {
                try
                {
                    // Lock the simulator to prevent other threads contacting it at the same time.
                    socketMutex.WaitOne();
                    // Request the value from the simulator.
                    telnetClient.Write("get " + varName + "\n");
                    // Save the value that has been sent from the simulator.
                    double value = HandleSimulatorReturn(varName);
                    return value;
                }
                catch (ServerNotConnectedException)
                {
                    NotifyError(ErrorMessages.errorsEnum.ServerDisconnected);
                    this.Disconnect();
                }
                catch (Exception e)
                {
                    NotifyError(ErrorMessages.errorsEnum.Other, e.Message);
                }

                finally
                {
                    // Done, release the simulator.
                    socketMutex.ReleaseMutex();
                }
            }
            returnVal = this.vars[varName].VarValue;
            return returnVal;
        }


        /// <summary>
        /// Gets the value from the simulator and handles ERR option by returning the current value - last good known.
        /// </summary>
        /// <param name="varName"> The var to get value of in case of ERR return value. </param>
        /// <returns> Value from the simulator if worked properly, otherwise the current saved value and error message. </returns>
        private double HandleSimulatorReturn(string varName)
        {
            string returnValue = telnetClient.Read();
            if (returnValue == "ERR" || returnValue == "ERR\n")
            {
                NotifyError(ErrorMessages.errorsEnum.ERRValue, varName);
                // Return the current value.
                return this.vars[varName].VarValue;
            }
            // Now check for any other error value:
            double result;
            try
            {
                result = Double.Parse(returnValue);
            }
            catch (Exception)
            {
                NotifyError(ErrorMessages.errorsEnum.InvalidValue, returnValue + " for var " + varName);
                // Return the current value.
                return this.vars[varName].VarValue;
            }
            // Else, we got a valid double number, return it.
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

        public void NotifyError(ErrorMessages.errorsEnum errorMessage, string additionalInfo = "")
        {
            ErrorOccurred?.Invoke(this, ErrorMessages.GetErrorMessage(errorMessage) + additionalInfo);
        }
    }
}
