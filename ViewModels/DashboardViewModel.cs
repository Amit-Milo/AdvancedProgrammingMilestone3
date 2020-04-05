using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModels
{
    /*
    class DashboardViewModel : IDashboardViewModel
    {
        // Save the ful names of the variables as used in the simulator.
        private const string headingDegName = "/instrumentation/heading-indicator/offset-deg";
        private const string verticalSpeedName = "/instrumentation/vertical-speed-indicator/indicated-speed-fpm";
        private const string groundSpeedName = "/instrumentation/gps/indicated-ground-speed-kt";
        private const string airSpeedName = "/instrumentation/airspeed-indicator/indicated-speed-kt";
        private const string gpsAltitudeName = "/instrumentation/gps/indicated-altitude-ft";
        private const string internalRollName = "/instrumentation/attitude-indicator/internal-roll-deg";
        private const string internalPitchName = "/instrumentation/attitude-indicator/internal-pitch-deg";
        private const string altimerAltitudeName = "/instrumentation/altimeter/indicated-altitude-ft";


        IFlightGearCommunicator model;

        // Data about the plane.
        private Dictionary<string, float> data;


        private float GetProperty(string key)
        {
            float DEFAULT_VALUE = 0;
            return this.data.ContainsKey("heading_deg") ? this.data["heading_deg"] : DEFAULT_VALUE;
        }


        private void SetProperty(string key, float value)
        {
            if (this.data.ContainsKey("heading_deg"))
                this.data["heading_deg"] = value;
            else
            {
                this.data.Add("heading_deg", value);
            }
        }


        // Properties referencing to the above variables.
        public float headingDeg
        {
            get { return this.GetProperty("heading_deg"); }
            set { this.SetProperty("heading_deg", value); }
        }
        public float verticalSpeed
        {
            get { return this.GetProperty("vertical_speed"); }
            set { this.SetProperty("vertical_speed", value); }
        }
        public float groundSpeed
        {
            get { return this.GetProperty("ground_speed"); }
            set { this.SetProperty("ground_speed", value); }
        }
        public float airSpeed
        {
            get { return this.GetProperty("air_speed"); }
            set { this.SetProperty("air_speed", value); }
        }
        public float gpsAltitude
        {
            get { return GetProperty("gps_altitude"); }
            set { this.SetProperty("gps_altitude", value); }
        }
        public float internalRoll
        {
            get { return this.GetProperty("internal_roll"); }
            set { this.SetProperty("internal_roll", value); }
        }
        public float internalPitch
        {
            get { return this.GetProperty("internal_pitch"); }
            set { this.SetProperty("internal_pitch", value); }
        }
        public float altimeterAltitude
        {
            get { return this.GetProperty("altimeter_altitude"); }
            set { this.SetProperty("altimeter_altitude", value); }
        }

        public DashboardViewModel(IFlightGearCommunicator model)
        {
            this.data = new Dictionary<string, float>();
            this.model = model;
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    if (sender == this.model)
                    {
                        string name = e.ChangedPropertyVar.VarName;

                        if (name.Equals(headingDegName))
                            headingDeg = e.ChangedPropertyVar.VarValue;
                        else if (name.Equals(verticalSpeedName))
                            verticalSpeed = e.ChangedPropertyVar.VarValue;
                        else if (name.Equals(groundSpeedName))
                            groundSpeed = e.ChangedPropertyVar.VarValue;
                        else if (name.Equals(airSpeedName))
                            airSpeed = e.ChangedPropertyVar.VarValue;
                        else if (name.Equals(gpsAltitudeName))
                            gpsAltitude = e.ChangedPropertyVar.VarValue;
                        else if (name.Equals(internalRollName))
                            internalRoll = e.ChangedPropertyVar.VarValue;
                        else if (name.Equals(internalPitchName))
                            internalPitch = e.ChangedPropertyVar.VarValue;
                        else if (name.Equals(altimeterAltitudeName))
                            altimeterAltitude = e.ChangedPropertyVar.VarValue;
                    }
                };
        }
    }*/
}
