using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#pragma warning disable CS0234 // The type or namespace name 'view' does not exist in the namespace 'FlightSimulatorApp' (are you missing an assembly reference?)
using FlightSimulatorApp.view;
#pragma warning restore CS0234 // The type or namespace name 'view' does not exist in the namespace 'FlightSimulatorApp' (are you missing an assembly reference?)
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
using System.Windows.Media.Animation;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
        }
    }

}
