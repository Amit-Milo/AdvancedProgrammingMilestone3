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

namespace FlightSimulatorApp.UserPanel {
    /// <summary>
    /// Interaction logic for UserMainPanel.xaml
    /// </summary>
    public partial class UserMainPanel : UserControl {
        static IConnectionPanelVM connectionVM;
        static IErrorsPanelMessager errorsVM;
        static IControllersPanelVM controellsVM;
        /// <summary>
        /// the user panel constructor. 
        /// the classes it contains need the program's model to be initialized, 
        /// so this constructor has a model parameter
        /// </summary>
        /// <param name="model"> the program's model </param>
        public UserMainPanel(IFlightGearCommunicator model) {
            InitializeComponent();

            //add the controllers panel
            controellsVM = new WaitingRoomControllersPanelVM(model);
            ControllersPanel controllersp = new ControllersPanel(controellsVM);
            this.RegisterName("controllersPanel", controllersp);
            mainUserPanel.Children.Add(controllersp);
            Grid.SetRow(controllersp, 0);

            //add the connections panel
            connectionVM = new ConnectionPanelVM(model);
            ConnectionPanel connectionp = new ConnectionPanel(connectionVM);
            mainUserPanel.Children.Add(connectionp);
            Grid.SetRow(connectionp, 1);

            //add the errors panel
            errorsVM = new ErrorsPanelVM(model);
            ErrorsPanel errorsPanel = new ErrorsPanel(errorsVM);
            mainUserPanel.Children.Add(errorsPanel);
            Grid.SetRow(errorsPanel, 2);
        }

        public void HandleJoystickMouseUp(object sender, MouseButtonEventArgs e) {
            (this.FindName("controllersPanel") as ControllersPanel).HandleJoystickMouseUp(sender, e);
        }
        
        public void HandleJoystickMouseMove(object sender, MouseEventArgs e) {
            (this.FindName("controllersPanel") as ControllersPanel).HandleJoystickMouseMove(sender, e);
        }



    }
}
