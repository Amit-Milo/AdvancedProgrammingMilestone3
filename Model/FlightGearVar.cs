using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    /// <summary>
    /// A small class that takes care of simulator variables' values in other classes.
    /// Each instance of this class handles one simulator variable.
    /// This class is observable. if the value changes, notify it's observers.
    /// </summary>
    public class FlightGearVar : INotifyPropertyChanged
    {
        /// <summary>
        /// Property of the variable's name. does not have a setter, because the name should not change.
        /// </summary>
        public string VarName
        {
            get;
        }

        private bool updateOnlyOnChange;
        /// <summary>
        /// The variable's value.
        /// </summary>
        private double varVal;
        /// <summary>
        /// Property of the variable's value. its setter also calls a notify observers function.
        /// </summary>
        public double VarValue
        {
            get
            {
                return varVal;
            }
            set
            {
                if (varVal != value || !updateOnlyOnChange)
                {
                    varVal = value;
                    // If the var's value changed, should notify about that to the observers.
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(VarName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="name"> The name of the variable. </param>
        /// <param name="value"> The value of the variable. </param>
        public FlightGearVar(string name, double value, bool updateOnlyOnChange = true)
        {
            this.VarName = name;
            this.VarValue = value;
            this.updateOnlyOnChange = updateOnlyOnChange;
        }
    }
}
