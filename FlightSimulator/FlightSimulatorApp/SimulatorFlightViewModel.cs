using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace FlightSimulatorApp
{

    class SimulatorFlightViewModel : INotifyPropertyChanged 
    {

        public double VM_Heading_Degree
        {
            get { return model.Heading_Degree; }
        }
        public double VM_Vertical_Speed
        {
            get { return model.Vertical_Speed; }
        }
        public double VM_Ground_Speed
        {
            get { return model.Ground_Speed; }
        }
        public double VM_Air_Speed
        {
            get { return model.Air_Speed; }
        }
        public double VM_Altitude_FT
        {
            get { return model.Altitude_FT; }
        }
        public double VM_Roll_Degree
        {
            get { return model.Roll_Degree; }
        }
        public double VM_Pitch_Degree
        {
            get { return model.Pitch_Degree; }
        }
        public double VM_Altimeter_FT
        {
            get { return model.Altimeter_FT; }
        }

        private double throttle;
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
        public double VM_Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                model.setAileron(aileron);
            }
        }

        private ISimulatorModel model;
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

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
