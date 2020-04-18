using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.DarkMode
{
    interface IDarkModeWithText : IDarkModeCapable
    {
        /// <summary>
        /// Change the color of the text in this view.
        /// </summary>
        /// <param name="b"> The new color of the text. </param>
        void ChangeTextColor(System.Windows.Media.Brush b);
    }
}
