using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Maps.MapControl.WPF;

using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.Map
{
    public class MapViewModel : IMapViewModel
    {
        private static readonly string latitudeName = "/position/latitude-deg";
        private static readonly string longitudeName = "/position/longitude-deg";

        private Location lastLocation = new Location(0,0);

        // A dictionary mapping each property name to the full variable name as saved at the server.
        private static readonly Dictionary<string,string> properties = new Dictionary<string,string>
        {
            { latitudeName, "Latitude" },
            { longitudeName, "Longitude" }
        };


        // Position variables.
        private double latitude = 0;
        private double longitude = 0;


        /// <summary>
        /// Access latitude variable. Make sure it is normalized.
        /// </summary>
        private double Latitude
        {
            get { return NormalizeLatitude(latitude); }
            set { latitude = NormalizeLatitude(value); }
        }


        /// <summary>
        /// Access longitude variable. Make sure it is normalized.
        /// </summary>
        private double Longitude
        {
            get { return NormalizeLatitude(longitude); }
            set { longitude = NormalizeLatitude(value); }
        }



        /// <summary>
        /// A propery in charge of notifying of position change.
        /// Notify only when both longitude and latitude are up-to-date.
        /// </summary>
        private byte positionUpdate;
        private byte PositionUpdate
        {
            set
            {
                // Update that one of the properties is updated. (LSB for altitude, next bit is longitude).
                positionUpdate |= value;

                // Check if both properties are up to date.
                if ((positionUpdate & 0x3) == 0x3 && !this.Position.Equals(lastLocation))
                {
                    // Reset poritionUpdate var.
                    positionUpdate = 0x0;

                    // Notify that properties have changed (location and angle).
                    NotifyPropertyChanged("Position");
                }
            }
        }


        /// <summary>
        /// The position of the plane.
        /// </summary>
        public Location Position
        {
            get
            {
                // Return current location based on latitude and longitude.
                return new Location(Latitude,Longitude);
            }
        }


        /// <summary>
        /// The angle in which the plane is directed.
        /// </summary>
        private double rotation = 0;
        public double Rotation
        {
            get
            {
                return this.rotation;
            }
            private set
            {
                this.rotation = value;
                NotifyPropertyChanged("Rotation");
            }
        }


        private static double CalculateRotation(Location p1,Location p2)
        {
            // Convert to radians.
            double lat1 = p1.Latitude * Math.PI / 180;
            double lat2 = p2.Latitude * Math.PI / 180;
            double long1 = p1.Longitude * Math.PI / 180;
            double long2 = p2.Longitude * Math.PI / 180;


            // The difference between the longitudes.
            double dLon = (long2 - long1);

            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1)
                    * Math.Cos(lat2) * Math.Cos(dLon);

            double brng = Math.Atan2(y,x);

            // Convert to radians and rotate the angle as the plane starts directed right.
            return brng * 180 / Math.PI - 90;
        }


        IFlightGearCommunicator model;

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Notify that a property changed to all listeners.
        /// </summary>
        /// <param name="property"> The name of the changed property. </param>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(property));
        }


        /// <summary>
        /// Normalize the latitude value. If the plane tries to leave map, don't change its value.
        /// </summary>
        /// <param name="candidate"> The expected next value. </param>
        /// <returns> The next value </returns>
        private double NormalizeLatitude(double candidate)
        {
            double MAX = 90, MIN = -90;
            return (candidate > MAX) ? MAX : (candidate < MIN) ? MIN : candidate;
        }


        /// <summary>
        /// Normalize the longitude value. If the plane tries to leave map, don't change its value.
        /// </summary>
        /// <param name="candidate"> The expected next value. </param>
        /// <returns> The next value </returns>
        private double NormalizeLongitude(double candidate)
        {
            return Location.NormalizeLongitude(candidate);
        }


        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="model"> The model, used for communicating with the server. </param>
        public MapViewModel(IFlightGearCommunicator model)
        {
            this.model = model;

            foreach (string var in properties.Keys)
                model.AddReceiveableVar(var,false);

            // Add the viewmodel to the model's listeners.
            this.model.PropertyChanged +=
                delegate (Object sender,PropertyChangedEventArgs e)
                {
                    string name = e.PropertyName;

                    // If latitude changed, update latitude.
                    if (name == latitudeName)
                    {
                        Latitude = this.model.GetVarValue(latitudeName);
                        PositionUpdate = 0x1;
                    }

                    // If longitude changed, update longitude.
                    else if (name == longitudeName)
                    {
                        Longitude = this.model.GetVarValue(longitudeName);
                        PositionUpdate = 0x2;
                    }
                };



            // Add self as a listener to the position change.
            this.PropertyChanged +=
                delegate (Object sender,PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "Position")
                    {
                        // Calculate the angle between the last position and the new one.
                        this.Rotation = CalculateRotation(lastLocation,Position);

                        // Update last location.
                        this.lastLocation = Position;
                    }
                };
        }
    }
}
