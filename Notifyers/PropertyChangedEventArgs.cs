using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlightSimulatorApp.Notifyers {
    /// <summary>
    /// A small class that holds an info about a property changed event.
    /// </summary>
    public class PropertyChangedEventArgs {
        /// <summary>
        /// The name of the property that changed.
        /// </summary>
        public FlightGearVar ChangedPropertyName { get; }
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="changedPropertyName"> the name of the property that changed </param>
        public PropertyChangedEventArgs(FlightGearVar changedPropertyName) {
            ChangedPropertyName = changedPropertyName;
        }
    }
}
