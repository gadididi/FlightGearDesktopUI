using FlightSimulatorApp.view;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for RunGame.xaml
    /// </summary>
    public partial class RunGame : Page
    {
        SimulatorFlightViewModel vm;
        public RunGame()
        {
            InitializeComponent();
            joystick1 = new Joystick();
            exit_button = new Button();

            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
