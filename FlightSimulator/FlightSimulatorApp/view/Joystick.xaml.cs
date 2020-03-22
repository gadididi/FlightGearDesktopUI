using System;
using System.Collections.Generic;
using System.Diagnostics;
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


namespace FlightSimulatorApp.view
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    /// 
    public partial class Joystick : UserControl
    {
        private Point _positionInBlock;
        private double offSetX;
        private double offSetY;
        private Rect myRectangle;
        private double recSize;
        private  static SimulatorFlightViewModel vm;
        private bool first_time = true;

        public Joystick()
        {
            InitializeComponent();
            recSize = 100;
        }

        public void Set_ViewModel(SimulatorFlightViewModel view)
        {
            vm = view;
        }



        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // when the mouse is down, get the position within the current control. (so the control top/left doesn't move to the mouse position)
            if (first_time) { 
                _positionInBlock = Mouse.GetPosition(this);
                first_time = false;

                Point position = this.knobBoarder.PointToScreen(new Point(0d, 0d)),
                controlPosition = this.PointToScreen(new Point(0d, 0d));


                offSetX = position.X;
                offSetY = position.Y;

                _positionInBlock = new Point(offSetX, offSetY);


                myRectangle = new Rect();
                myRectangle.Location = new Point(position.X - 50, position.Y - 50);
                myRectangle.Size = new Size(recSize, recSize);
            }

            // capture the mouse (so the mouse move events are still triggered (even when the mouse is not above the control)
            this.CaptureMouse();
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            // if the mouse is captured. you are moving it. (there is your 'real' boolean)
            if (this.IsMouseCaptured)
            {
                // get the parent container
                var container = this.knobBoarder as UIElement;

                // get the position within the container
                var mousePosition = e.GetPosition(container);

                double XPos = mousePosition.X + myRectangle.Left - this.knobBoarder.ActualWidth / 4;
                double YPos = mousePosition.Y + myRectangle.Top - this.knobBoarder.ActualHeight / 4;


                // move the usercontrol.
                if (myRectangle.Left > XPos)
                {
                    if (myRectangle.Top < YPos && myRectangle.Bottom > YPos)
                    {
                        Knob.RenderTransform = new TranslateTransform(myRectangle.Left - offSetX, YPos - offSetY);
                        Movement_Translation(myRectangle.Left, YPos);
                    }
                }
                else if (myRectangle.Right < XPos)
                {
                    if (myRectangle.Top < YPos && myRectangle.Bottom > YPos)
                    {
                        Knob.RenderTransform = new TranslateTransform(myRectangle.Right - offSetX, YPos - offSetY);
                        Movement_Translation(myRectangle.Right, YPos);
                    }
                }
                else if (myRectangle.Top > YPos)
                {
                    if (myRectangle.Right > XPos && myRectangle.Left < XPos)
                    {
                        Knob.RenderTransform = new TranslateTransform(XPos - offSetX, myRectangle.Top - offSetY);
                        Movement_Translation(XPos, myRectangle.Top);
                    }
                }
                else if (myRectangle.Bottom < YPos)
                {
                    if (myRectangle.Right > XPos && myRectangle.Left < XPos)
                    {
                        Knob.RenderTransform = new TranslateTransform(XPos - offSetX, myRectangle.Bottom - offSetY);
                        Movement_Translation(XPos, myRectangle.Bottom);
                    }
                }
                else if (myRectangle.Contains(new Point(XPos, YPos)))
                {
                    Knob.RenderTransform = new TranslateTransform(XPos - offSetX, YPos - offSetY);
                    Movement_Translation(XPos, YPos);
                }
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // release this control.
            this.ReleaseMouseCapture();
            Knob.RenderTransform = new TranslateTransform();
        }

        private void Movement_Translation(double x, double y)
        {
            double deltaX = (x - offSetX) / recSize * 2;
            double deltaY = (y - offSetY) / recSize * -2;
            Debug.WriteLine("deltaX,deltaY: " + deltaX + "," + deltaY);

            try
            {
                vm.setDirection(deltaX, deltaY);
            } catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
            }
            
        }
    }
}
