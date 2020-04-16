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
using System.Configuration;
using System.Collections.Specialized;
using FlightSimulatorApp.UserPanel;
using FlightSimulatorApp.UserPanel.Connection;

namespace FlightSimulatorApp.UserPanel.Connection {
    /// <summary>
    /// Interaction logic for ConnectionPanel.xaml
    /// </summary>
    public partial class ConnectionPanel : UserControl {

        private IConnectionPanelVM vm;

        /// <summary>
        /// constructor for the connection panel view. 
        /// gets the vm and thus needs to be initialized in the code and not in the xaml.
        /// </summary>
        /// <param name="vm"> the View's VM </param>
        public ConnectionPanel(IConnectionPanelVM viewModel) {
            InitializeComponent();
            this.vm = viewModel;
            DataContext = vm;
        }

        /// <summary>
        /// handle event of connect button click
        /// </summary>
        private void Connect_Click(object sender, RoutedEventArgs e) {
            this.vm.Connect();
        }

        /// <summary>
        /// handle event of disconnect button click
        /// </summary>
        private void Disconnect_Click(object sender, RoutedEventArgs e) {
            this.vm.Disconnect();
        }
    }
}
