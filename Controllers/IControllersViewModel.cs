using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Controllers {
    public interface IControllersViewModel {
        void SetSimVar(string varName, double value);
    }
}
