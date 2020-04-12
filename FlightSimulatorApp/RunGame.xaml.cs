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
    /// Interaction logic for RunGame.xaml
    public partial class RunGame : Page
    {
        //ViewModel
        SimulatorFlightViewModel vm;
        public static bool to_main;
        //Intiallize the RunGame component
        public RunGame()
        {
            InitializeComponent();
            exit_button = new Button();
            //Set ViewModel and Model
            TcpClient tcpClient = new TcpClient();
            this.vm = new SimulatorFlightViewModel(new SimulatorModel(tcpClient));
            to_main = false;

            //Set Joystick
            joystick1 = new Joystick();
            joystick1.Set_ViewModel(this.vm);

            DataContext = vm; //Set DataContext
        }

        //Exit button click: return to main page
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.vm.Disconnect();
            if (NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
                to_main = true;
                errlog.Text = "";
            }
        }

        //Sets the IP and port for connection
        public void Set_IP_Port(string ip, string port)
        {
            int Port = Int32.Parse(port);
            this.vm.set_ip_and_port(ip, Port);
        }

        //Center the map around the airplane
        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myMap.Center = myPin.Location;
            }
            catch
            {
                Debug.WriteLine("Map hasn't been able to center around the plane");
            }
        }

        //When the error log updates, show different dialogs (pop-up) and terminate the connection
        private void errlog_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (errlog.Text == "Conection Timeout") //TimeOut
            {
                return;
            }
            else if (errlog.Text == "TCP conection to the server failed") //TCP handshake failed
            {
                this.vm.Disconnect();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

                if (NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }

                return;
            }
            else if (errlog.Text == "The connection was forcibly closed by the remote host, please go back to the main menu and try again")
            {
                this.vm.Disconnect();

                if (NavigationService.CanGoBack)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                    this.NavigationService.GoBack();
                }
                return;
            }
        }
    }
}