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

namespace FlightSimulatorApp.UserPanel
{
    /// <summary>
    /// Interaction logic for UserMainPanel.xaml
    /// </summary>
    public partial class UserMainPanel : UserControl
    {
        static IConnectionPanelVM connectionVM;
        static IErrorsPanelMessager errorsVM;
        static IControllersPanelVM controellsVM;

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
            ControllersPanel controllersp = new ControllersPanel(controllersPanelVM);
            controllersp.MouseUpOccurredReleaseJoystick +=
                delegate (object sender, MouseButtonEventArgs e)
                {
                    this.HandleJoystickMouseUp(sender, e);
                };
            this.RegisterName("controllersPanel", controllersp);
            mainUserPanel.Children.Add(controllersp);
            Grid.SetRow(controllersp, 0);

            // Add the connections panel.
            ConnectionPanel connectionp = new ConnectionPanel(connectionPanelVM);
            connectionp.MouseUpOccurredReleaseJoystick +=
                delegate (object sender, MouseButtonEventArgs e)
                {
                    this.HandleJoystickMouseUp(sender, e);
                };
            mainUserPanel.Children.Add(connectionp);
            Grid.SetRow(connectionp, 1);

            // Add the errors panel.
            ErrorsPanel errorsPanel = new ErrorsPanel(errorsPanelVM);
            mainUserPanel.Children.Add(errorsPanel);
            Grid.SetRow(errorsPanel, 2);
        }


        /// <summary>
        /// A mouse up event happened. forward it to the joystick. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleJoystickMouseUp(object sender, MouseButtonEventArgs e)
        {
            (this.FindName("controllersPanel") as ControllersPanel).HandleJoystickMouseUp(sender, e);
        }

        /// <summary>
        /// A mouse move event happened. forward it to the joystick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleJoystickMouseMove(object sender, MouseEventArgs e)
        {
            (this.FindName("controllersPanel") as ControllersPanel).HandleJoystickMouseMove(sender, e);
        }

    }
}
