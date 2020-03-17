using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlightSimulatorApp.view;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            joystick1 = new Joystick();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void joystick1_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
