using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp.Map
{
    /// <summary>
    /// View model in the mvvm architecture.
    /// Act as a view for the model and as a model for the map view.
    /// </summary>
    public interface IMapViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The position of the plane relatively to the map layout.
        /// </summary>
        Location Position
        {
            get;
        }


        /// <summary>
        /// The angle in which the plane is directed.
        /// </summary>
        double Rotation
        {
            get;
        }

        /// <summary>
        /// The current velocity of te plane.
        /// </summary>
        double Velocity
        {
            get;
        }
    }
}
