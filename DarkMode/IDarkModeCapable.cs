using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulatorApp.DarkMode
{
    /// <summary>
    /// This interface handles view objects that can be set to dark mode.
    /// </summary>
    public interface IDarkModeCapable
    {
        /// <summary>
        /// Set the view's dark mode option on.
        /// </summary>
        void SetDarkModeOn(object sender = null, RoutedEventArgs e = null);
        /// <summary>
        /// Set the view's dark mode option off.
        /// </summary>
        void SetDarkModeOff(object sender = null, RoutedEventArgs e = null);
    }
}
