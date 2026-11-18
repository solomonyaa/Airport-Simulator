using Planes.Common;
using Planes.UI.stationUpdateSvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Planes.UI
{
    public partial class MainWindow : Window
    {
        private readonly StationsUpdaterServiceClient stationsUpdater;
        private readonly List<Station> stations;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            stationsUpdater = new StationsUpdaterServiceClient();
            this.stations = new List<Station>();
            stations.AddRange(stationsUpdater.GetStations());
            Task.Factory.StartNew(UpdateStations);
            //Task.Factory.StartNew(ResetStation9);
        }
        private BitmapImage GetImage(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.EndInit();
            return image;
        }
        private Task UpdateStations()
        {
            while (true)
            {
                bool changed = false;
                var newStations = this.stationsUpdater.GetStations();
                for (int i = 0; i < newStations.Count; i++)
                {
                    if (this.stations[i].CurrentPlane == null)
                    {
                        if (newStations[i].CurrentPlane != null)
                        {
                            this.Dispatcher.Invoke(() => Station_PropertyChanged(newStations[i], newStations[i].CurrentPlane.StartingStationID));
                            changed = true;
                        }
                    }
                    else
                    {
                        if (!this.stations[i].CurrentPlane.Equals(newStations[i].CurrentPlane))
                        {
                            this.Dispatcher.Invoke(() => Station_PropertyChanged(newStations[i], stations[i].CurrentPlane.StartingStationID));
                            changed = true;
                        }
                    }
                }
                if (changed)
                {
                    this.stations.Clear();
                    this.stations.AddRange(newStations);
                }
                //Thread.Sleep(1000);
            }
        }
        private void Station_PropertyChanged(object sender, int startingStationId)
        {
            var station = sender as Station;
            var sp = this.AirportGrid.FindName($"station{station.Id}SP");

            if (sp == null)
                return;

            if (station.CurrentPlane == null)
            {
                ((sp as StackPanel).Children[0] as Image).Source = null;
                ((sp as StackPanel).Children[1] as TextBlock).Text = null;

                ((AirportGrid.FindName($"station9SP") as StackPanel).Children[0] as Image).Source = null;

                if (station.Id == 4 && startingStationId != 1)
                {
                    ((AirportGrid.FindName($"station9SP") as StackPanel).Children[0] as Image).Source = GetImage("departure_plane.png");
                    //((AirportGrid.FindName($"station9SP") as StackPanel).Children[1] as TextBlock).Text = station.CurrentPlane.ToString();
                }
            }
            else
            {
                var path = startingStationId == 1 ? "arrival_plane.png" : "departure_plane.png";
                ((sp as StackPanel).Children[0] as Image).Source = GetImage(path);
                ((sp as StackPanel).Children[1] as TextBlock).Text = station.CurrentPlane.ToString();
            }
        }
        //private Task ResetStation9()
        //{
        //    while (true)
        //    {
        //        ((AirportGrid.FindName($"station9SP") as StackPanel).Children[0] as Image).Source = null;
        //        Thread.Sleep(10000);
        //    }
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FlightsWindow window = new FlightsWindow(this);
            window.Show();
            this.Hide();
        }
    }
}
