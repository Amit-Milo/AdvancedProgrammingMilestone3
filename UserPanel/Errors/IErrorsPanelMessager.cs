using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp.UserPanel.Errors
{
    /// <summary>
    /// Interface for Errors panel VM. this VM should handle errors and notify V on changes.
    /// </summary>
    public interface IErrorsPanelMessager : IErrorHandler, INotifyPropertyChanged
    {
        /// <summary>
        /// the error message to be displayed on the screen.
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
