using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp.UserPanel.Controllers
{
    /// <summary>
    /// Interaction logic for ControllersPanel.xaml
    /// </summary>
    public partial class ControllersPanel : UserControl, INotifyMouseUp
    {
        private IControllersPanelVM vm;

        public event MouseUpOnElement MouseUpOccurredReleaseJoystick;

        /// <summary>
        /// Constructor for the controllers panel view. 
        /// Gets the vm and thus needs to be initialized in the code and not in the xaml.
        /// </summary>
        /// <param name="vm"> The View's VM. </param>
        public ControllersPanel(IControllersPanelVM vm)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = vm;
            // Handle event of disconnection: set sliders to 0.
            this.vm.DisconnectionOccurred +=
                delegate ()
                {
                    ThrottleSlider.Value = 0;
                    AileronSlider.Value = 0;
                };
        }


        /// <summary>
        /// Called at an event of mouse up at any place on the screen from the MainWindow.
        /// Should call the joystick's method to handle this event.
        /// </summary>
        public void HandleJoystickMouseUp(object sender, MouseButtonEventArgs e)
        {
            (this.FindName("joyStickPanel") as Joystick).HandleJoystickMouseUp(sender, e);
        }

        /// <summary>
        /// Called at an event of mouse move at any place on the screen from the MainWindow.
        /// Should call the joystick's method to handle this event.
        /// </summary>
        public void HandleJoystickMouseMove(object sender, MouseEventArgs e)
        {
            (this.FindName("joyStickPanel") as Joystick).HandleJoystickMouseMove(sender, e);
        }

        private void NotifyMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUpOccurredReleaseJoystick?.Invoke(sender, e);
        }
    }
}
