using FlightSimulatorApp.Notifiers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model {
    /// <summary>
    /// A small class that takes care of simulator variables' values in other classes.
    /// Each instance of this class handles one simulator variable.
    /// This class is observable. if the value changes, notify it's observers.
    /// </summary>
    public class FlightGearVar : INotifyPropertyChanged {
        /// <summary>
        /// Property of the variable's name. does not have a setter, because the name should not change.
        /// </summary>
        public string VarName { get; }
        /// <summary>
        /// The variable's value
        /// </summary>
        private double varVal;
        /// <summary>
        /// Property of the variable's value. it's setter also calls a notify observers function.
        /// </summary>
        public double VarValue {
            get {
                return varVal;
            }
            set {
                if (varVal != value) {
                    varVal = value;
                    //if the var's value changed, should notify about that to the observers.
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(VarName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="name"> the name of the variable </param>
        /// <param name="value"> the value of the variable </param>
        public FlightGearVar(string name, double value) {
            this.VarName = name;
            this.VarValue = value;
        }
    }
}
