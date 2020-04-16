using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.Dashboard
{
    
    public class DashboardViewModel : IDashboardViewModel
    {
        // Save the full names of the variables as used in the simulator.
        private static readonly string headingDegName = "/instrumentation/heading-indicator/indicated-heading-deg";
        private static readonly string verticalSpeedName = "/instrumentation/gps/indicated-vertical-speed";
        private static readonly string groundSpeedName = "/instrumentation/gps/indicated-ground-speed-kt";
        private static readonly string airSpeedName = "/instrumentation/airspeed-indicator/indicated-speed-kt";
        private static readonly string gpsAltitudeName = "/instrumentation/gps/indicated-altitude-ft";
        private static readonly string internalRollName = "/instrumentation/attitude-indicator/internal-roll-deg";
        private static readonly string internalPitchName = "/instrumentation/attitude-indicator/internal-pitch-deg";
        private static readonly string altimeterAltitudeName = "/instrumentation/altimeter/indicated-altitude-ft";

        // A dictionary mapping each propery name to the full variable name as saved at the server.
        private static readonly Dictionary<string, string> properties = new Dictionary<string, string>
        {
            { headingDegName, "HeadingDeg" },
            { verticalSpeedName, "VerticalSpeed" },
            { groundSpeedName, "GroundSpeed" },
            { airSpeedName, "AirSpeed" },
            { gpsAltitudeName, "GpsAltitude" },
            { internalRollName, "InternalRoll" },
            { internalPitchName, "InternalPitch" },
            { altimeterAltitudeName, "AltimeterAltitude" }
        };

        public event PropertyChangedEventHandler PropertyChanged;

        // The odel communicating with the server.
        private readonly IFlightGearCommunicator model;


        /// <summary>
        /// Get the value of a property.
        /// </summary>
        /// <param name="key"> The name of the property. </param>
        /// <returns> The value associated with the property. </returns>
        private double GetProperty(string key)
        {
            double DefaultValue = 0;

            // Using the mapping dict in a reversed way. Find the key associated with the property name.
            string keyName = properties.FirstOrDefault(x => x.Value == key).Key;
            try
            {
                return this.model.GetVarValue(keyName);
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }


        /// <summary>
        /// Notify that a property changed to all listeners.
        /// </summary>
        /// <param name="property"> The name of the changed property. </param>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }


        // Properties referencing to the above variables.
        public double HeadingDeg
        {
            get { return this.GetProperty("HeadingDeg"); }
        }
        public double VerticalSpeed
        {
            get { return this.GetProperty("VerticalSpeed"); }
        }
        public double GroundSpeed
        {
            get { return this.GetProperty("GroundSpeed"); }
        }
        public double AirSpeed
        {
            get { return this.GetProperty("AirSpeed"); }
        }
        public double GpsAltitude
        {
            get { return GetProperty("GpsAltitude"); }
        }
        public double InternalRoll
        {
            get { return this.GetProperty("InternalRoll"); }
        }
        public double InternalPitch
        {
            get { return this.GetProperty("InternalPitch"); }
        }
        public double AltimeterAltitude
        {
            get { return this.GetProperty("AltimeterAltitude"); }
        }


        /// <summary>
        /// THe constructor.
        /// </summary>
        /// <param name="model"> The model, used for communicating with the server. </param>
        public DashboardViewModel(IFlightGearCommunicator model)
        {
            this.model = model;

            // Add the viewmodel to the model's listeners.
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    string name = e.PropertyName;
                    if (properties.ContainsKey(name))
                        NotifyPropertyChanged(properties[name]);
                };

            foreach (string name in properties.Keys)
                this.model.AddReceiveableVar(name);
        }
    }
}
