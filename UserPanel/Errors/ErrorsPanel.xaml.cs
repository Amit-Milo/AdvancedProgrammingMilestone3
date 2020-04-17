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
using FlightSimulatorApp.UserPanel.Errors;

namespace FlightSimulatorApp.UserPanel.Errors
{
    /// <summary>
    /// Interaction logic for ErrorsPanel.xaml
    /// </summary>
    public partial class ErrorsPanel : UserControl
    {
        private IErrorsPanelMessager vm;

        /// <summary>
        /// Constructor for the errors panel view. 
        /// Gets the vm and thus needs to be initialized in the code and not in the xaml.
        /// </summary>
        /// <param name="vm"> The View's VM. </param>
        public ErrorsPanel(IErrorsPanelMessager vm)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = vm;
        }
    }
}
