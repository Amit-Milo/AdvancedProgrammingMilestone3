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
using FlightSimulatorApp.UserPanel.Connection;
using FlightSimulatorApp.UserPanel.Controllers;
using FlightSimulatorApp.UserPanel.Errors;

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
        public IConnectionPanelVM connectionVM;
        public IErrorsPanelMessager errorsVM;
        public IControllersPanelVM controellsVM;


        private void Application_Startup(object sender, StartupEventArgs args)
        {
            ITelnetClient telnetClient = new TelnetClient();

            model = new FlightGearCommunicator(telnetClient);
            model.Connect(ConfigurationManager.AppSettings.Get("DEFAULT_IP"), int.Parse(ConfigurationManager.AppSettings.Get("DEFAULT_PORT")));
            model.Start();

            dashboardViewModel = new DashboardViewModel(model);
            mapViewModel = new MapViewModel(model);

            connectionVM = new ConnectionPanelVM(model);
            errorsVM = new ErrorsPanelVM(model);
            controellsVM = new ControllersPanelVM(model);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                this.model.Disconnect();
            }
            catch
            {

            }
        }
    }
}
