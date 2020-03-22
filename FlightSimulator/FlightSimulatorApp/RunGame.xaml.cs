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
using System.Net.Sockets;
using System.Diagnostics;
using Microsoft.Maps.MapControl.WPF;

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
            exit_button = new Button();
            TcpClient tcpClient = new TcpClient();
            this.vm = new SimulatorFlightViewModel(new SimulatorModel(tcpClient));
            joystick1 = new Joystick();
            joystick1.Set_ViewModel(this.vm);
            DataContext = vm;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }
        public void Set_IP_Port(string ip, string port)
        {
            int Port = Int32.Parse(port);
            this.vm.set_ip_and_port(ip, Port);
        }

    }
}
