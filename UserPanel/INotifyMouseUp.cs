using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulatorApp.UserPanel
{
    /// <summary>
    /// This interface takes care of handling the invoking of mouse up event for the joystick
    /// on other wpf elements such as buttons and sliders.
    /// </summary>
    public interface INotifyMouseUp
    {
        /// <summary>
        /// The delegate event that will be executed when needed.
        /// </summary>
        event MouseUpOnElement MouseUpOccurredReleaseJoystick;

    }

    /// <summary>
    /// Definition of the delegate function that should be used inside the interface.
    /// </summary>
    public delegate void MouseUpOnElement(object sender, MouseButtonEventArgs e);
}
