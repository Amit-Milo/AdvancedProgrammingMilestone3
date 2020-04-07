using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp {
    /// <summary>
    /// This interface takes care of the observable pattern.
    /// Each class that implements this interface is an obervable, 
    /// and its observers can access its delegate event field and add functions that with be executed when the observable changes.
    /// </summary>
    public interface INotifyPropertyChanged {
        /// <summary>
        /// The delegate event that will be executed when needed.
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// call the PropertyChanged event.
        /// </summary>
        /// <param name="flightGearVar"> the variable that called that notification, its value changed </param>
        void NotifyPropertyChanged(FlightGearVar flightGearVar);
    }

    /// <summary>
    /// Definition of the delegate function that should be used inside the interface.
    /// </summary>
    /// <param name="sender"> the object that called the notification </param>
    /// <param name="e"> information about the notification </param>
    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);
}
