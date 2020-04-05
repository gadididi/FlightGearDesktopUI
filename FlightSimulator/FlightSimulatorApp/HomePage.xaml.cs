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
using System.Diagnostics;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        //run is the next page
        RunGame run;

        //CTOR
        public HomePage()
        {
            InitializeComponent();
        }

        //exit click ,exit the game
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        // click fly start the game only if the port and ip standard , else show  "wrong IP or port"
        private void Button_Click_Fly(object sender, RoutedEventArgs e)
        {
           
            if (Check_ip() && Check_port()) // !!!! OR IP AND PORT IS GOOD
            {
                this.run = new RunGame();

                try
                {
                    run.Set_IP_Port(ip.Text, port.Text);
                    this.NavigationService.Navigate(run);
                }
                catch
                {
                    mistake.Text = "Connection failed";
                }
            } else
            {
                mistake.Text = "  wrong IP or port";
            }
        }
        //the user in the text box of ip
        private void ip_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ip.Text == "Enter IP")
            {
                ip.Text = "";
            }
        }
        //the user in the text port box
        private void port_GotFocus(object sender, RoutedEventArgs e)
        {
            if (port.Text == "Enter port")
            {
                port.Text = "";
            }
        }

        // the user leave the ip box empty , return the diffualt sentence
        private void ip_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ip.Text == "")
            {
                ip.Text = "Enter IP";
            }
        }

        // the user leave the port box empty , return the Default sentence
        private void port_LostFocus(object sender, RoutedEventArgs e)
        {
            if (port.Text == "")
            {
                port.Text = "Enter port";
            }
        }

        // get the default ip + port from the app config file
        private void def_Click(object sender, RoutedEventArgs e)
        {
            string myIP = ConfigurationManager.AppSettings["ip"];
            string myPort = ConfigurationManager.AppSettings["port"];
            port.Text = myPort;
            ip.Text = myIP;
        }

        //checking if the ip is standard
        private bool Check_ip()
        {
            string phrase = this.ip.Text;
            string[] numbers = phrase.Split('.');
            int i;
            for(i=0; i < 4; i++)
            {
                try
                {
                    if(Int32.Parse(numbers[i]) <0 || Int32.Parse(numbers[i])>255)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    // show not valid port on the screen
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
            if (numbers.Length > 4)
            {
                return false;
            }
            return true;
        }
        //checking if the port is standard
        private bool Check_port()
        {
            int m;
            try
            {
                m = Int32.Parse(port.Text);
            }
            catch (FormatException e)
            {
                // show not valid port on the screen
                Debug.WriteLine(e.Message);
                return false;
            }
            if (m < 0 || (m >= 0 && m <= 1023) || m > 65535)
            {
                return false;
            }
            return true;
        }
    }
}