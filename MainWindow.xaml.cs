using FlightSimulatorApp.Model;
using FlightSimulatorApp.Dashboard;
using FlightSimulatorApp.UserPanel.Errors;
using FlightSimulatorApp.UserPanel;
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

namespace FlightSimulatorApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            List<string> vars = new List<string>();
            vars.Add("not_in");
            vars.Add("/instrumentation/heading-indicator/offset-deg");
            ITelnetClient client = new TelnetClient();
            IFlightGearCommunicator m = new Model.Model(client, vars);
            m.Connect("127.0.0.1", 5402);
            m.Start();

            UserMainPanel userMainPanel = new UserMainPanel(m);
            this.RegisterName("userPanel", userMainPanel);
            mainGrid.Children.Add(userMainPanel);
            Grid.SetColumn(userMainPanel, 1);
            Grid.SetRow(userMainPanel, 1);

            IDashboardViewModel vm = new DashboardViewModel(m);
            DashboardView dashboard = new DashboardView(vm);
            mainGrid.Children.Add(dashboard);
            Grid.SetColumn(dashboard, 1);
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e) {
            (this.FindName("userPanel") as UserMainPanel).HandleJoystickMouseUp(sender, e);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e) {
            (this.FindName("userPanel") as UserMainPanel).HandleJoystickMouseMove(sender, e);
        }
    }
}
