using FlightSimulatorApp.DarkMode;
using FlightSimulatorApp.UserPanel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace FlightSimulatorApp.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl, IDarkModeWithText
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="viewModel"> The viewmodel associated with the dashboard. </param>
        public DashboardView(IDashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }


        public void SetDarkModeOff(object sender = null, RoutedEventArgs e = null)
        {
            this.dashboardGrid.Background = System.Windows.Media.Brushes.White;
            this.ChangeTextColor(System.Windows.Media.Brushes.Black);
        }

        public void SetDarkModeOn(object sender = null, RoutedEventArgs e = null)
        {
            byte darkness = 30;
            this.dashboardGrid.Background = new SolidColorBrush(Color.FromRgb(darkness, darkness, darkness));
            this.ChangeTextColor(System.Windows.Media.Brushes.White);
        }

        public void ChangeTextColor(System.Windows.Media.Brush b)
        {
            foreach (object l in dashboardGrid.Children)
            {
                if (l is Label)
                {
                    (l as Label).Foreground = b;
                }
                
            }
        }
    }
}
