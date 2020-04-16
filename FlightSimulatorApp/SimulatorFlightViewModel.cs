using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;
using System.Diagnostics;

namespace FlightSimulatorApp
{
    public class SimulatorFlightViewModel : INotifyPropertyChanged
    {
        //The model
        private readonly ISimulatorModel Model;

        //Latitude and Longitude
        public string VM_LongtitudeLatitude
        {
            get
            {
                return Model.LongtitudeLatitude;
            }
        }

        //Error log property
        public string VM_Errlog
        {
            get
            {
                return Model.Errlog;
            }
        }

        //The location of the plane
        public Location VM_Location
        {
            get {
                return Model.Location; }
        }

        //Heading by degree
        public double VM_HeadingDegree
        {
            get { return Model.HeadingDegree; }
        }

        //Vertical speed by KT units
        public double VM_VerticalSpeed
        {
            get { return Model.VerticalSpeed; }
        }

        //Ground speed by KT units
        public double VM_GroundSpeed
        {
            get { return Model.GroundSpeed; }
        }

        //Air speed by KT units
        public double VM_AirSpeed
        {
            get { return Model.AirSpeed; }
        }

        //Altitude by FT units
        public double VM_AltitudeFT
        {
            get { return Model.AltitudeFT; }
        }
        
        //Roll by degree
        public double VM_RollDegree
        {
            get { return Model.RollDegree; }
        }

        //Pitch by degree
        public double VM_PitchDegree
        {
            get { return Model.PitchDegree; }
        }
        //Altimeter by FT units
        public double VM_AltimeterFT
        {
            get { return Model.AltimeterFT; }
        }

        //Longtitude by degree
        public double VM_LongitudeDegree
        {
            get { return Model.LongitudeDegree; }
        }

        //Latitude by degree
        public double VM_LatitudeDegree
        {
            get { return Model.LatitudeDegree; }
        }

        private double Throttle;
        //Throttle in the interval [0,1]
        public double VM_Throttle
        {
            get { return Throttle; }
            set
            {
                Throttle = value;
                Model.SetThrottle(Throttle);
            }
        }

        private double Aileron;
        //Aileron in the interval [-1,1]
        public double VM_Aileron
        {
            get { return Aileron; }
            set
            {
                Aileron = value;
                Model.SetAileron(Aileron);
            }
        }

        //Gets the values for the rudder and elevator and send to the model.
        public void SetDirection(double XRudder, double YElevator)
        {
            Model.SetDirection(XRudder, YElevator);
        }

        //ViewModel CTOR
        public SimulatorFlightViewModel(ISimulatorModel m)
        {
            this.Model = m;
            Model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;

        //Notify upon property changed event
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        //Send the IP and port to the model
        public void SetIPAndPort(string ip, int port)
        {
            this.Model.Connect(ip, port);
        }

        //Disconnect from the server.
        public void Disconnect()
        {
            this.Model.Disconnect();
        }
    }
}