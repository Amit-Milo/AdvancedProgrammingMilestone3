using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    /// <summary>
    /// A custom exception indicating that a server is not connected.
    /// </summary>
    class ServerNotConnectedException : IOException
    {
    }
}
