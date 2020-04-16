using FlightSimulatorApp.Model;
using FlightSimulatorApp.Dashboard;
using FlightSimulatorApp.Map;
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
using FlightSimulator;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var app = Application.Current as App;

            DashboardView dashboard = new DashboardView(app.dashboardViewModel);
            MapView map = new MapView(app.mapViewModel);
            UserMainPanel userMainPanel = app.mainPanel;

            this.RegisterName("userPanel", userMainPanel);

            mainGrid.Children.Add(dashboard);
            mainGrid.Children.Add(map);
            mainGrid.Children.Add(userMainPanel);

            Grid.SetColumn(dashboard, 1);
            Grid.SetRowSpan(map, 2);
            Grid.SetColumn(userMainPanel, 1);
            Grid.SetRow(userMainPanel, 1);
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            (this.FindName("userPanel") as UserMainPanel).HandleJoystickMouseUp(sender, e);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            (this.FindName("userPanel") as UserMainPanel).HandleJoystickMouseMove(sender, e);
        }
    }
}
