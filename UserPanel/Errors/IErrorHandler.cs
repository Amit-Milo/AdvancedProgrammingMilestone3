using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.UserPanel.Errors {
    /// <summary>
    /// This interface takes care of handling errors.
    /// </summary>
    public interface IErrorHandler {
        /// <summary>
        /// Handle the given error.
        /// </summary>
        /// <param name="error"> the given error </param>
        void HandleError(string error);
    }
}
