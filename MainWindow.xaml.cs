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

namespace Ex1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            List<string> vars = new List<string>();
            vars.Add("/instrumentation/gps/indicated-vertical-speed");
            ITelnetClient client = new TelnetClient();
            IFlightGearCommunicator m = new Model(client, vars);
            m.Connect("127.0.0.1", 5402);
            m.Start();
        }
    }
}
