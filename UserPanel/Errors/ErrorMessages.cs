using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.UserPanel.Errors
{
    /// <summary>
    /// This class contains the errors notification syntax.
    /// </summary>
    public class ErrorMessages
    {

        /// <summary>
        /// The errors possibilities. Use these for a cleaner, clearer and more consistent code.
        /// </summary>
        public enum errorsEnum
        {
            Other, Unknown,
            InavlidIP, InvalidPort, InvalidPortAndIP,
            ERRValue, InvalidValue,
            TimeOut, UnconnectedServer, ServerDisconnected,
            ValueOutsideRange

        }

        /// <summary>
        /// A mapping between the errors enum and the errors messages.
        /// </summary>
        private static Dictionary<errorsEnum, string> errorMessages = new Dictionary<errorsEnum, string>
        {
            {errorsEnum.Other,"" },
            {errorsEnum.Unknown,"" },
            {errorsEnum.InavlidIP,"invalid IP address" },
            {errorsEnum.InvalidPort,"invalid PORT" },
            {errorsEnum.InvalidPortAndIP,"invalid port and ip" },
            {errorsEnum.ERRValue,"got ERR value " },
            {errorsEnum.InvalidValue,"got invalid value " },
            {errorsEnum.TimeOut,"server timeout" },
            {errorsEnum.UnconnectedServer,"server is not connected" },
            {errorsEnum.ServerDisconnected,"server disconnected" },
            {errorsEnum.ValueOutsideRange,"got value outside range " }
        };

        /// <summary>
        /// The UI of this class. when a user enters an enum value, it returns the corresponding error message.
        /// </summary>
        /// <param name="e"> The error type enum. </param>
        /// <returns> The error message string of the input enum error. </returns>
        public static string GetErrorMessage(errorsEnum e)
        {
            return errorMessages[e];
        }


    }
}
