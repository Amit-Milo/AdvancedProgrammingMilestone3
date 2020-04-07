using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Notifyers {
    /// <summary>
    /// This interface takes care of the observable pattern.
    /// Each class that implements this interface is an obervable, 
    /// and its observers can access its delegate event field and add functions that with be executed when the observable changes.
    /// this interface takes care of messaging in events of errors.
    /// </summary>
    public interface INotifyError {
        /// <summary>
        /// The delegate event that will be executed when needed.
        /// </summary>
        event ErrorNotification ErrorOccurred;
        /// <summary>
        /// call the ErrorOccurred event.
        /// </summary>
        /// <param name="error"> the error message to show </param>
        void NotifyErrorOccurred(string error);
    }
    /// <summary>
    /// Definition of the delegate function that should be used inside the interface.
    /// </summary>
    /// <param name="sender"> the object that called the notification </param>
    /// <param name="error"> the error message that is sent </param>
    public delegate void ErrorNotification(object sender, string error);
}
