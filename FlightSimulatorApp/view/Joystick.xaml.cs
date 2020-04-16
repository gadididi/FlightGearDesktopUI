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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FlightSimulatorApp.view
{
    /// Interaction logic for Joystick.xaml
    public partial class Joystick : UserControl
    {
        private double OffSetX;
        private double OffSetY;
        private Rect MyRectangle;
        private readonly double RectangleSize;
        private static SimulatorFlightViewModel ViewModel;
        private bool FirstTime = true;
        private readonly Storyboard Story;

        //CTOR
        public Joystick()
        {
            InitializeComponent();
            RectangleSize = 100;
            this.Story = Knob.FindResource("CenterKnob") as Storyboard;
        }

        //Set the view model for this view (Joystick)
        public void SetViewModel(SimulatorFlightViewModel view)
        {
            ViewModel = view;
        }


        //While mouse down on the joystick
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // when the mouse is down, get the position within the current control. (so the control top/left doesn't move to the mouse position)
            if (FirstTime)
            {
                FirstTime = false;

                //Get Absolute position of the knob
                Point position = this.knobBoarder.PointToScreen(new Point(0d, 0d));

                //Set offset to help translate relative position in the knob to the absolute position.
                OffSetX = position.X;
                OffSetY = position.Y;

                //Create a rectangle to be the boarder of the knob
                MyRectangle = new Rect();
                MyRectangle.Location = new Point(position.X - 50, position.Y - 50);
                MyRectangle.Size = new Size(RectangleSize, RectangleSize);
            }

            // capture the mouse (so the mouse move events are still triggered (even when the mouse is not above the control)
            this.CaptureMouse();
        }

        //What happed when the mouse moves
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            // if the mouse is captured. you are moving it. (there is your 'real' boolean)
            if (this.IsMouseCaptured)
            {
                // get the knobBoarder container
                var container = this.knobBoarder as UIElement;

                // get the position within the container
                var mousePosition = e.GetPosition(container);

                //Translate the relative position of the mouse into an absolute one.
                double XPos = mousePosition.X + MyRectangle.Left - this.knobBoarder.ActualWidth / 4;
                double YPos = mousePosition.Y + MyRectangle.Top - this.knobBoarder.ActualHeight / 4;


                // move the usercontrol.
                if (MyRectangle.Left > XPos) //mouse is outside the boarder from the left
                {
                    if (MyRectangle.Top < YPos && MyRectangle.Bottom > YPos)
                    {
                        //Knob.RenderTransform = new TranslateTransform(MyRectangle.Left - OffSetX, YPos - OffSetY);
                        knobPosition.X = MyRectangle.Left - OffSetX;
                        knobPosition.Y = YPos - OffSetY;
                        MovementTranslation(MyRectangle.Left, YPos);
                    }
                }
                else if (MyRectangle.Right < XPos) //mouse is outside the boarder from the right
                {
                    if (MyRectangle.Top < YPos && MyRectangle.Bottom > YPos)
                    {
                        //Knob.RenderTransform = new TranslateTransform(MyRectangle.Right - OffSetX, YPos - OffSetY);
                        knobPosition.X = MyRectangle.Right - OffSetX;
                        knobPosition.Y = YPos - OffSetY;
                        MovementTranslation(MyRectangle.Right, YPos);
                    }
                }
                else if (MyRectangle.Top > YPos)
                {
                    if (MyRectangle.Right > XPos && MyRectangle.Left < XPos) //mouse is outside the boarder from the top
                    {
                        //Knob.RenderTransform = new TranslateTransform(XPos - OffSetX, MyRectangle.Top - OffSetY);
                        knobPosition.X = XPos - OffSetX;
                        knobPosition.Y = MyRectangle.Top - OffSetY;
                        MovementTranslation(XPos, MyRectangle.Top);
                    }
                }
                else if (MyRectangle.Bottom < YPos) //mouse is outside the boarder from the bottom
                {
                    if (MyRectangle.Right > XPos && MyRectangle.Left < XPos)
                    {
                        //Knob.RenderTransform = new TranslateTransform(XPos - OffSetX, MyRectangle.Bottom - OffSetY);
                        knobPosition.X = XPos - OffSetX;
                        knobPosition.Y = MyRectangle.Bottom - OffSetY;
                        MovementTranslation(XPos, MyRectangle.Bottom);
                    }
                }
                else if (MyRectangle.Contains(new Point(XPos, YPos))) //mouse is inside the boarder
                {
                    //Knob.RenderTransform = new TranslateTransform(XPos - OffSetX, YPos - OffSetY);
                    knobPosition.X = XPos - OffSetX;
                    knobPosition.Y = YPos - OffSetY;
                    MovementTranslation(XPos, YPos);
                }
            }
        }

        //Release the mouse and return knob to the default position
        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // release this control.
            this.ReleaseMouseCapture();
            this.Story.Begin();
            //Knob.RenderTransform = new TranslateTransform();
        }

        //translate the X,Y position into values from the unit rectangle
        private void MovementTranslation(double x, double y)
        {
            double deltaX = (x - OffSetX) / RectangleSize * 2;
            double deltaY = (y - OffSetY) / RectangleSize * -2;
            //Debug.WriteLine("deltaX,deltaY: " + deltaX + "," + deltaY);

            try
            {
                ViewModel.SetDirection(deltaX, deltaY);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            Story.Stop();
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }
    }
}