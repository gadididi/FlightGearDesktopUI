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
using System.Configuration;


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void Button_Click_Fly(object sender, RoutedEventArgs e)
        {
            RunGame run = new RunGame();
            this.NavigationService.Navigate(run);

        }
        private void ip_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ip.Text == "Enter IP")
            {
                ip.Text = "";
            }
        }

        private void port_GotFocus(object sender, RoutedEventArgs e)
        {
            if (port.Text == "Enter port")
            {
                port.Text = "";
            }
        }

        private void ip_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ip.Text == "")
            {
                ip.Text = "Enter IP";
            }
        }

        private void port_LostFocus(object sender, RoutedEventArgs e)
        {
            if (port.Text == "")
            {
                port.Text = "Enter port";
            }
        }

        private void def_Click(object sender, RoutedEventArgs e)
        {
            string myIP = ConfigurationManager.AppSettings["ip"];
            string myPort = ConfigurationManager.AppSettings["port"];

            //add vm.set ip port!!!#####
        }
    }
}
