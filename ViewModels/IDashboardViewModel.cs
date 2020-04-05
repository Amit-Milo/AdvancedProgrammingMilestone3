using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModels
{
    interface IDashboardViewModel
    {
        float headingDeg { get; set; }
        float verticalSpeed { get; set; }
        float groundSpeed { get; set; }
        float airSpeed { get; set; }
        float gpsAltitude { get; set; }
        float internalRoll { get; set; }
        float internalPitch { get; set; }
        float altimerAltitude { get; set; }
    }
}
