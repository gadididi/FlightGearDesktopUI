using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace FlightSimulatorApp
{
    class SimulatorFlightViewModel : INotifyPropertyChanged 
    {
        private ISimulatorModel model;
        public SimulatorFlightViewModel(ISimulatorModel m)
        {
            this.model = m;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
