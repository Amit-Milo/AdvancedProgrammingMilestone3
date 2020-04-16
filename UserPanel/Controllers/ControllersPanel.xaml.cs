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

namespace FlightSimulatorApp.UserPanel.Controllers {
    /// <summary>
    /// Interaction logic for ControllersPanel.xaml
    /// </summary>
    public partial class ControllersPanel : UserControl {
        private IControllersPanelVM vm;
        /// <summary>
        /// constructor for the controllers panel view. 
        /// gets the vm and thus needs to be initialized in the code and not in the xaml.
        /// </summary>
        /// <param name="vm"> the View's VM </param>
        public ControllersPanel(IControllersPanelVM vm) {
            InitializeComponent();
            this.vm = vm;
            DataContext = vm;
        }

        /// <summary>
        /// called at an event of mouse up at any place on the screen from the MainWindow.
        /// should call the joystick's method to handle this event.
        /// </summary>
        public void HandleJoystickMouseUp(object sender, MouseButtonEventArgs e) {
            (this.FindName("joyStickPanel") as Joystick).HandleJoystickMouseUp(sender, e);
        }

        /// <summary>
        /// called at an event of mouse move at any place on the screen from the MainWindow.
        /// should call the joystick's method to handle this event.
        /// </summary>
        public void HandleJoystickMouseMove(object sender, MouseEventArgs e) {
            (this.FindName("joyStickPanel") as Joystick).HandleJoystickMouseMove(sender, e);
        }
    }
}
