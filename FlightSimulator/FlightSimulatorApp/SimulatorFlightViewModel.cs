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
        private ISimulatorModel model;

        //Latitude and Longitude
        public string VM_Long_lat
        {
            get
            {
                return model.Long_lat;
            }
        }

        //Error log property
        public string VM_Errlog
        {
            get
            {
                return model.Errlog;
            }
        }

        //The location of the plane
        public Location VM_Location
        {
            get {
                return model.Location; }
        }

        //Heading by degree
        public double VM_Heading_Degree
        {
            get { return model.Heading_Degree; }
        }

        //Vertical speed by KT units
        public double VM_Vertical_Speed
        {
            get { return model.Vertical_Speed; }
        }

        //Ground speed by KT units
        public double VM_Ground_Speed
        {
            get { return model.Ground_Speed; }
        }

        //Air speed by KT units
        public double VM_Air_Speed
        {
            get { return model.Air_Speed; }
        }

        //Altitude by FT units
        public double VM_Altitude_FT
        {
            get { return model.Altitude_FT; }
        }
        
        //Roll by degree
        public double VM_Roll_Degree
        {
            get { return model.Roll_Degree; }
        }

        //Pitch by degree
        public double VM_Pitch_Degree
        {
            get { return model.Pitch_Degree; }
        }
        //Altimeter by FT units
        public double VM_Altimeter_FT
        {
            get { return model.Altimeter_FT; }
        }

        //Longtitude by degree
        public double VM_Longitude_deg
        {
            get { return model.Longitude_deg; }
        }

        //Latitude by degree
        public double VM_Latitude_deg
        {
            get { return model.Latitude_deg; }
        }

        private double throttle;
        //Throttle in the interval [0,1]
        public double VM_Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                model.setThrottle(throttle);
            }
        }

        private double aileron;
        //Aileron in the interval [-1,1]
        public double VM_Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                model.setAileron(aileron);
            }
        }

        //Gets the values for the rudder and elevator and send to the model.
        public void setDirection(double x_rudder, double y_elevator)
        {
            model.setDirection(x_rudder, y_elevator);
        }

        //ViewModel CTOR
        public SimulatorFlightViewModel(ISimulatorModel m)
        {
            this.model = m;
            model.PropertyChanged +=
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
        public void set_ip_and_port(string ip, int port)
        {
            this.model.Connect(ip, port);
        }

        //Disconnect from the server.
        public void Disconnect()
        {
            this.model.Disconnect();
        }
    }
}