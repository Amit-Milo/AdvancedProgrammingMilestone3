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

using FlightSimulatorApp.Dashboard;

namespace FlightSimulatorApp.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        private IDashboardViewModel vm;


        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="viewModel"> The viewmodel associated with the dashboard. </param>
        public DashboardView(IDashboardViewModel viewModel)
        {
            InitializeComponent();

            this.vm = viewModel;

            DataContext = vm;
        }
    }
}
