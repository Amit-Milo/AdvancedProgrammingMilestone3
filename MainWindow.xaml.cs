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
using FlightSimulatorApp.DarkMode;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDarkModeCapable
    {
        private DashboardView dashboard;
        private MapView map;
        private UserMainPanel userMainPanel;

        public MainWindow()
        {
            InitializeComponent();

            var app = Application.Current as App;

            this.dashboard = new DashboardView(app.dashboardViewModel);
            this.map = new MapView(app.mapViewModel);
            this.userMainPanel = new UserMainPanel(app.controellsVM, app.connectionVM, app.errorsVM);

            mainGrid.Children.Add(this.dashboard);
            mainGrid.Children.Add(this.userMainPanel);

            Grid.SetColumn(this.dashboard, 1);
            Grid.SetColumn(this.userMainPanel, 1);
            Grid.SetRow(this.userMainPanel, 1);

            leftSide.Children.Add(this.map);
            Grid.SetRow(this.map, 1);

            this.SetDarkModeOff();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.userMainPanel.HandleJoystickMouseUp(sender, e);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            this.userMainPanel.HandleJoystickMouseMove(sender, e);
        }


        public void SetDarkModeOn(object sender = null, RoutedEventArgs e = null)
        {
            darkModeButton.Content = "Dark mode is on\nClick to set dark mode OFF";
            settingsPanel.Background = System.Windows.Media.Brushes.LightGray;
            this.userMainPanel.SetDarkModeOn();
            this.dashboard.SetDarkModeOn();
        }

        public void SetDarkModeOff(object sender = null, RoutedEventArgs e = null)
        {
            darkModeButton.Content = "Dark mode is off\nClick to set dark mode ON";
            settingsPanel.Background = System.Windows.Media.Brushes.White;
            this.userMainPanel.SetDarkModeOff();
            this.dashboard.SetDarkModeOff();
        }
    }
}
