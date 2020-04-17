using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp.Map
{
    public interface IMapViewModel : INotifyPropertyChanged
    {
        Location Position
        {
            get;
        }
        double Rotation
        {
            get;
        }
    }
}
