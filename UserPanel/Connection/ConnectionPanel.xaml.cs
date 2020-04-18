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
using FlightSimulatorApp.DarkMode;

namespace FlightSimulatorApp.UserPanel.Connection
{
    /// <summary>
    /// Interaction logic for ConnectionPanel.xaml
    /// </summary>
    public partial class ConnectionPanel : UserControl, INotifyMouseUp, IDarkModeWithText
    {

        private IConnectionPanelVM vm;

        /// <summary>
        /// Constructor for the connection panel view. 
        /// Gets the vm and thus needs to be initialized in the code and not in the xaml.
        /// </summary>
        /// <param name="vm"> The View's VM. </param>
        public ConnectionPanel(IConnectionPanelVM viewModel)
        {
            InitializeComponent();
            this.vm = viewModel;
            DataContext = vm;
        }

        public event MouseUpOnElement MouseUpOccurredReleaseJoystick;

        
        public void SetDarkModeOff(object sender = null, RoutedEventArgs e = null)
        {
            this.connectionPanel.Background = System.Windows.Media.Brushes.White;
            this.ChangeTextColor(System.Windows.Media.Brushes.Black);
        }

        public void SetDarkModeOn(object sender = null, RoutedEventArgs e = null)
        {
            byte darkness = 65;
            this.connectionPanel.Background = new SolidColorBrush(Color.FromRgb(darkness, darkness, darkness));
            this.ChangeTextColor(System.Windows.Media.Brushes.White);
        }

        public void ChangeTextColor(Brush b)
        {
            foreach (object tb in connectionPanel.Children)
            {
                if (tb is TextBlock)
                {
                    (tb as TextBlock).Foreground = b;
                }
            }
        }


        /// <summary>
        /// Handle event of connect button click.
        /// </summary>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            this.vm.ConnectAndStart();
        }

        /// <summary>
        /// Handle event of disconnect button click.
        /// </summary>
        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            this.vm.Disconnect();
        }

        private void NotifyMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUpOccurredReleaseJoystick?.Invoke(sender, e);
        }
    }
}
