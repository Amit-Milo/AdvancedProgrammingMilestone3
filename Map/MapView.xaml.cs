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

using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp.Map
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView(IMapViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            viewModel.PropertyChanged +=
                delegate (object sender, PropertyChangedEventArgs e)
                {
                    try
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            double a = 5.8, b = 1.3;
                            if (e.PropertyName == "Velocity")
                                map.SetView(viewModel.Position, a - b * Math.Log(viewModel.Velocity));
                        });
                    }
                    catch
                    {
                    }
                };
        }
    }
}
