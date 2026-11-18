using Planes.UI.FlightControlSvc;
using System.Windows;
using System.Windows.Controls;

namespace Planes.UI
{
    public partial class FlightsWindow : Window
    {
        private readonly FlightControlServiceClient flightControl;
        private readonly MainWindow window;
        public FlightsWindow(MainWindow window)
        {
            InitializeComponent();
            flightControl = new FlightControlServiceClient();
            this.window = window;
        }
        private void PopulateGrid(bool isArrival)
        {
            var flights = this.flightControl.GetAllFlights(isArrival);
            var listview = isArrival ? mastergrid.FindName("arrivalListView") : mastergrid.FindName("depListView");
            //var fix = isArrival ? 42 : 142;
            foreach (var flight in flights)
            {
                var sp = new StackPanel { Orientation = Orientation.Horizontal };
                sp.Children.Add(new Label { Content = flight.FlightId, Width = 120 });
                sp.Children.Add(new Label { Content = flight.Date, Width = 150 });
                sp.Children.Add(new Label { Content = flight.Plane.Id, Width = 120 });
                (listview as ListView).Items.Add(new ListViewItem { Content = sp });
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateGrid(true);
            PopulateGrid(false);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            window.Show();
            this.Close();
        }
    }
}
