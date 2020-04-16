using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.UserPanel.Errors
{
    public class ErrorMessages
    {

        public enum errorsEnum
        {
            Other, Unknown,
            InavlidIP, InvalidPort, InvalidPortAndIP,
            ERRValue, InvalidValue,
            TimeOut, UnconnectedServer, ServerDisconnected,
            ValueOutsideRange

        }

        private static Dictionary<errorsEnum,string> errorMessages = new Dictionary<errorsEnum,string>
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


        public static string GetErrorMessage(errorsEnum e)
        {
            return errorMessages[e];
        }


    }
}
