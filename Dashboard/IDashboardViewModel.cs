using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Dashboard
{
    /// <summary>
    /// View model in the mvvm architecture.
    /// Act as a view for the model and as a model for the dashboard view.
    /// </summary>
    public interface IDashboardViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Properties associated with the dashboard.
        /// </summary>
        double HeadingDeg
        {
            get;
        }
        double VerticalSpeed
        {
            get;
        }
        double GroundSpeed
        {
            get;
        }
        double AirSpeed
        {
            get;
        }
        double GpsAltitude
        {
            get;
        }
        double InternalRoll
        {
            get;
        }
        double InternalPitch
        {
            get;
        }
        double AltimeterAltitude
        {
            get;
        }
    }
}
