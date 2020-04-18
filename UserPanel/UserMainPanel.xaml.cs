using FlightSimulatorApp.UserPanel.Connection;
using FlightSimulatorApp.UserPanel.Errors;
using FlightSimulatorApp.UserPanel.Controllers;
using FlightSimulatorApp.Model;
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
using FlightSimulatorApp.DarkMode;


namespace FlightSimulatorApp.UserPanel
{
    /// <summary>
    /// Interaction logic for UserMainPanel.xaml
    /// </summary>
    public partial class UserMainPanel : UserControl,IDarkModeCapable
    {
        private ControllersPanel controllersp;
        private ConnectionPanel connectionp;
        private ErrorsPanel errorsPanel;

        /// <summary>
        /// The constructor.
        /// Gets it's components.
        /// </summary>
        /// <param name="controllersPanelVM"></param>
        /// <param name="connectionPanelVM"></param>
        /// <param name="errorsPanelVM"></param>
        public UserMainPanel(IControllersPanelVM controllersPanelVM, IConnectionPanelVM connectionPanelVM, IErrorsPanelMessager errorsPanelVM)
        {
            InitializeComponent();
            // Add the controllers panel.
            this.controllersp = new ControllersPanel(controllersPanelVM);
            this.controllersp.MouseUpOccurredReleaseJoystick +=
                delegate (object sender, MouseButtonEventArgs e)
                {
                    this.HandleJoystickMouseUp(sender, e);
                };
            mainUserPanel.Children.Add(this.controllersp);
            Grid.SetRow(this.controllersp, 0);

            // Add the connections panel.
            this.connectionp = new ConnectionPanel(connectionPanelVM);
            this.connectionp.MouseUpOccurredReleaseJoystick +=
                delegate (object sender, MouseButtonEventArgs e)
                {
                    this.HandleJoystickMouseUp(sender, e);
                };
            mainUserPanel.Children.Add(this.connectionp);
            Grid.SetRow(this.connectionp, 1);

            // Add the errors panel.
            this.errorsPanel = new ErrorsPanel(errorsPanelVM);
            mainUserPanel.Children.Add(this.errorsPanel);
            Grid.SetRow(this.errorsPanel, 2);
        }


        /// <summary>
        /// A mouse up event happened. forward it to the joystick. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleJoystickMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.controllersp.HandleJoystickMouseUp(sender, e);
        }

        /// <summary>
        /// A mouse move event happened. forward it to the joystick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleJoystickMouseMove(object sender, MouseEventArgs e)
        {
            this.controllersp.HandleJoystickMouseMove(sender, e);
        }

        public void SetDarkModeOn(object sender = null, RoutedEventArgs e = null)
        {
            this.errorsPanel.SetDarkModeOn();
            this.controllersp.SetDarkModeOn();
            this.connectionp.SetDarkModeOn();
        }

        public void SetDarkModeOff(object sender = null, RoutedEventArgs e = null)
        {
            this.errorsPanel.SetDarkModeOff();
            this.controllersp.SetDarkModeOff();
            this.connectionp.SetDarkModeOff();
        }
    }
}
