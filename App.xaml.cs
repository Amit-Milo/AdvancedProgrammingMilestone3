using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using FlightSimulatorApp.Dashboard;
using FlightSimulatorApp.Map;
using FlightSimulatorApp.Model;
using FlightSimulatorApp.UserPanel;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IFlightGearCommunicator model;
        public IDashboardViewModel dashboardViewModel;
        public IMapViewModel mapViewModel;
        public UserMainPanel mainPanel;


        private void Application_Startup(object sender, StartupEventArgs args)
        {
            ITelnetClient telnetClient = new TelnetClient();

            model = new Model(telnetClient);
            model.Connect(ConfigurationManager.AppSettings.Get("DEFAULT_IP"), int.Parse(ConfigurationManager.AppSettings.Get("DEFAULT_PORT")));

            dashboardViewModel = new DashboardViewModel(model);
            mapViewModel = new MapViewModel(model);
            mainPanel = new UserMainPanel(model);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            this.model.Disconnect();
        }
    }
}
