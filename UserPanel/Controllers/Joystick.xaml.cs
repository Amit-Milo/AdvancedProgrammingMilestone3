﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace FlightSimulatorApp.UserPanel.Controllers
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {

        private bool isJoystickBeingHeld;
        private Point knobHoldingPointRelativeToUpperLeftCorner;
        private Point knobHoldingPointRelativeToKnobCenter;

        // Define the normalized knob position x,y to be binded using dependency property with the joystick's simulator vars.
        public double NormalizedKnobX
        {
            get => (double)GetValue(NormalizedKnobXProperty);
            set => SetValue(NormalizedKnobXProperty, value);
        }
        public static readonly DependencyProperty NormalizedKnobXProperty = DependencyProperty.Register(nameof(NormalizedKnobX), typeof(double), typeof(Joystick));
        public double NormalizedKnobY
        {
            get => (double)GetValue(NormalizedKnobYProperty);
            set => SetValue(NormalizedKnobYProperty, value);
        }
        public static readonly DependencyProperty NormalizedKnobYProperty = DependencyProperty.Register(nameof(NormalizedKnobY), typeof(double), typeof(Joystick));


        /// <summary>
        /// The constructor.
        /// Set the joystick to being free,
        /// And init the knobHoldingPoint properties.
        /// </summary>
        public Joystick()
        {
            InitializeComponent();
            this.isJoystickBeingHeld = false;
            // Init the points with garbage default values.
            this.knobHoldingPointRelativeToUpperLeftCorner = new Point(0, 0);
            this.knobHoldingPointRelativeToKnobCenter = new Point(0, 0);
        }

        /// <summary>
        /// Event that happens when the knob is released, and it's animation of coming back to 0,0 ends.
        /// </summary>
        private void centerKnob_Completed(object sender, EventArgs e)
        {
            Storyboard storyBoard = Knob.FindResource("CenterKnob") as Storyboard;
            storyBoard.Stop();
            this.SetKnobX(0);
            this.SetKnobY(0);
        }

        /// <summary>
        /// This is called when a mouse up event happens anywhere on the screen, from the MainWindow,
        /// To allow holding the joystick outside of it's shape.
        /// If the joystick was held, start the coming back to 0,0 animation, and set joystick to be free.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleJoystickMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.isJoystickBeingHeld)
            {
                //start animation of returning knob to 0,0.
                Storyboard storyBoard = Knob.FindResource("CenterKnob") as Storyboard;
                storyBoard.Begin();
                //joystick is not being held anymore, set this boolean to false. it will be false again when mouse up event happens.
                this.isJoystickBeingHeld = false;
            }
        }

        /// <summary>
        /// This is called when a mouse move event happens anywhere on the screen, from the MainWindow,
        /// To allow holding the joystick outside of it's shape.
        /// If the joystick is held, calculate the new knob position in relation to the holding knob position.
        /// </summary>
        public void HandleJoystickMouseMove(object sender, MouseEventArgs e)
        {
            // Do all of that only if the joystick is being held, otherwise do nothing.
            if (isJoystickBeingHeld)
            {
                double borderCircleRadius = (this.Base).RenderSize.Width / 2;
                double knobCircleRadius = (this.KnobBase).RenderSize.Width / 2;
                // Now get the new planned knob center relative to the big border circle center.
                double newPlannedKnobX = e.GetPosition(this.Base).X - borderCircleRadius - this.knobHoldingPointRelativeToKnobCenter.X;
                double newPlannedKnobY = e.GetPosition(this.Base).Y - borderCircleRadius - this.knobHoldingPointRelativeToKnobCenter.Y;
                /*
                 * Now operate like that:
                 * check if this means the knob is inside of the border circle.
                 * if it is:
                 *      just put the knob in the position
                 * if it is not:
                 *      calculate the slope of the line that passes through the circle center and the new knob position.
                 *      calculate the point on this line that has a distance 
                 *      of (radius of the border circle - radius of the knob) from the circle center,
                 *      at the direction of the curr mouse point. set the knob position to this place.
                 */
                // Check if the whole knob is inside the border circle when it's center is the knew center.
                if (Distance(newPlannedKnobX, newPlannedKnobY, 0, 0) <= borderCircleRadius - knobCircleRadius)
                {
                    // Just set it to be the new center point.
                    this.SetKnobX(newPlannedKnobX);
                    this.SetKnobY(newPlannedKnobY);
                }
                else
                {
                    /* Calculate the slope. the center of the knob placing circle's equation is
                     * x^2+y^2=(R-r)^2 where R is the radius of the border circle and r is the radius of the knob.*/
                    // First, treat the undefined slope case.
                    if (newPlannedKnobX == 0)
                    {
                        // It is a vertical line. 
                        double newYAbs = borderCircleRadius - knobCircleRadius;
                        // Check if the point should be above or below the current one.
                        if (newPlannedKnobY > 0)
                        {
                            // Above
                            this.SetKnobY(newYAbs);
                        }
                        else
                        {
                            // Below
                            this.SetKnobY(-newYAbs);
                        }
                    }
                    else
                    {
                        // Get the slope of the line.
                        double slope = newPlannedKnobY / newPlannedKnobX;
                        // Now, the intersection of y=m*x and x^2+y^2=(R-r)^2 is at x=(R-r)/sqrt(1+m^2).
                        double newKnobXAbs = (borderCircleRadius - knobCircleRadius) / Math.Sqrt(1 + slope * slope);
                        // Get the sign of the new x.
                        double newKnobX = newKnobXAbs;
                        if (slope == 0)
                        {
                            if (newPlannedKnobX < 0)
                            {
                                newKnobX = -newKnobX;
                            }
                            // Else keep it positive
                        }
                        else if ((slope > 0 && newPlannedKnobY < 0) || (slope < 0 && newPlannedKnobY > 0))
                        {
                            newKnobX = -newKnobX;
                        }
                        // Else keep it positive.
                        // Now get the y.
                        double newKnobY = slope * newKnobX;
                        // And finally, set the values.
                        this.SetKnobX(newKnobX);
                        this.SetKnobY(newKnobY);
                    }

                }
            }

        }

        /// <summary>
        /// The joystick is now being held. calculate the holding position vars and set the holding boolean to true.
        /// </summary>
        private void KnobBase_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Set the holding point delta. 
            // Set values relative to the upper left corner.
            this.knobHoldingPointRelativeToUpperLeftCorner.X = e.GetPosition(this.KnobBase).X;
            this.knobHoldingPointRelativeToUpperLeftCorner.Y = e.GetPosition(this.KnobBase).Y;
            // Set values relative to knob center. 
            // Need to subtract the radius because the GetPosition function returns position relative to the upper left corner.
            this.knobHoldingPointRelativeToKnobCenter.X = e.GetPosition(this.KnobBase).X - (this.KnobBase).RenderSize.Width / 2;
            this.knobHoldingPointRelativeToKnobCenter.Y = e.GetPosition(this.KnobBase).Y - (this.KnobBase).RenderSize.Height / 2;
            // Set the being held boolean to true. it will be false again when mouse up event happens.
            this.isJoystickBeingHeld = true;
        }

        /// <summary>
        /// Calculate the distance between two points.
        /// </summary>
        /// <param name="x1"> The first point's x. </param>
        /// <param name="y1"> The first point's y. </param>
        /// <param name="x2"> The second point's x. </param>
        /// <param name="y2"> The second point's y. </param>
        /// <returns> The distance between the points. </returns>
        private static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        /// <summary>
        /// Change the joystick's x position,
        /// And the normalized x position for the simulator var that is related to the x value.
        /// </summary>
        /// <param name="value"> The new x value of the joystick. </param>
        private void SetKnobX(double value)
        {
            this.knobPosition.X = value;
            this.NormalizedKnobX = value / (this.Base.RenderSize.Width / 2 - this.KnobBase.Width / 2);
        }

        /// <summary>
        /// Change the joystick's y position,
        /// And the normalized y position for the simulator var that is related to the y value.
        /// </summary>
        /// <param name="value"> The new y value of the joystick. </param>
        private void SetKnobY(double value)
        {
            this.knobPosition.Y = value;
            // Set to negative because the pixels y axis is upside down.
            this.NormalizedKnobY = -value / (this.Base.RenderSize.Height / 2 - this.KnobBase.Height / 2);
        }
    }
}
