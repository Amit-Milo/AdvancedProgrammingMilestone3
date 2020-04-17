using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    /// <summary>
    /// This interface takes care of returning unreceivable vars to default values
    /// </summary>
    public interface INotifyDisconnectionOccurred
    {
        /// <summary>
        /// The delegate event that will be executed when needed.
        /// </summary>
        event DisconnectionNotification DisconnectionOccurred;
    }

    /// <summary>
    /// Definition of the delegate function that should be used inside the interface.
    /// </summary>
    public delegate void DisconnectionNotification();
}
