using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex1.Controllers {
    interface IControllersViewModel {
        Tuple<double, double> CalculateJoyStickXY(int xPixel, int yPixel);
    }
}
