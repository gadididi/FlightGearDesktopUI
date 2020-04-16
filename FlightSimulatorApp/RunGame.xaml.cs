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
        readonly SimulatorFlightViewModel ViewModel;
        public static bool GoToMainMenu;
        //Intiallize the RunGame component
        public RunGame()
        {
            InitializeComponent();
            ExitButton = new Button();
            //Set ViewModel and Model
            TcpClient tcpClient = new TcpClient();
            this.ViewModel = new SimulatorFlightViewModel(new SimulatorModel(tcpClient));
            GoToMainMenu = false;

            //Set Joystick
            joystick1 = new Joystick();
            joystick1.SetViewModel(this.ViewModel);

            DataContext = ViewModel; //Set DataContext
        }

        //Exit button click: return to main page
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Disconnect();
            if (NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
                GoToMainMenu = true;
                Errlog.Text = "";
            }
        }

        //Sets the IP and port for connection
        public void SetIPAndPort(string ip, string port)
        {
            int Port = Int32.Parse(port);
            this.ViewModel.SetIPAndPort(ip, Port);
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
        private void Errlog_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (Errlog.Text == "Conection Timeout") //TimeOut
            {
                return;
            }
            else if (Errlog.Text == "TCP conection to the server failed") //TCP handshake failed
            {
                this.ViewModel.Disconnect();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

                if (NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }

                return;
            }
            else if (Errlog.Text == "The connection was forcibly closed by the remote host, please go back to the main menu and try again")
            {
                this.ViewModel.Disconnect();

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