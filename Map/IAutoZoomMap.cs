using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulatorApp.Map
{
    public interface IAutoZoomMap
    {
        /// <summary>
        /// Turn on the map's auto zoom.
        /// </summary>
        void TurnOnAutoZoom(object sender = null, RoutedEventArgs e = null);

        /// <summary>
        /// Turn off the map's auto zoom.
        /// </summary>
        void TurnOffAutoZoom(object sender = null, RoutedEventArgs e = null);
    }
}
