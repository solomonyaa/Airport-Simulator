using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planes.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planes.UnitTests
{
    [TestClass]
    public class AirportTesting
    {
        private readonly List<Station> Stations;
        public AirportTesting()
        {
            this.Stations = new List<Station>();

            // build routes by design showed in exercise
            var station8 = new Station(8);
            var station7 = new Station(7, null, new List<Station> { station8 });
            var station6 = new Station(6, null, new List<Station> { station8 });
            var station5 = new Station(5, new List<Station> { station7, station6 });
            var station4 = new Station(4, new List<Station> { station5 });
            var station3 = new Station(3, new List<Station> { station4 });
            var station2 = new Station(2, new List<Station> { station3 });
            var station1 = new Station(1, new List<Station> { station2 });
            station8.DepartureNextStations.Add(4);

            this.Stations.AddRange(new List<Station>
                            { station1, station2, station3, station4, station5, station6, station7,station8 });

        }
        [TestInitialize]
        public void Cleanup()
        {
            foreach (var station in this.Stations)
            {
                station.CurrentPlane = null;
            }
        }
        [TestMethod]
        public void TestUnavailableStation()
        {
            var station = this.Stations.Find((s) => s.Id == 1);
            Plane p = new Plane(1, 60);
            station.SetPlane(p);
            Plane p2 = new Plane(2, 60);
            station.SetPlane(p2);
            Assert.AreEqual(station.CurrentPlane, p);
        }
        [TestMethod]
        public void TestAvailableStation()
        {
            var station = this.Stations.Find((s) => s.Id == 1);
            Plane p = new Plane(1, 60);
            Assert.AreEqual(station.CurrentPlane, null);

            station.SetPlane(p);
            Assert.AreEqual(station.CurrentPlane, p);
        }
        [TestMethod]
        public void TestMovingPlaneBetweenStations()
        {
            Plane p = new Plane(1, 2);
            p.StartingStationID = 1;
            Plane p2 = new Plane(2, 4);
            p2.StartingStationID = 1;
            var station2 = this.Stations.Find((s) => s.Id == 2);

            var station1 = this.Stations.Find((s) => s.Id == 1);
            station1.SetPlane(p, MoveToNextStation);
            station2.SetPlane(p2, MoveToNextStation);
            Task.Delay(new TimeSpan(0, 0, 3)).Wait(); // wait 4 seconds

            Assert.AreEqual(station1.CurrentPlane, p);
        }

        [TestMethod]
        public void MovePlaneWhenAvailable()
        {
            Plane p = new Plane(1, 2);
            p.StartingStationID = 1;
            Plane p2 = new Plane(2, 4);
            p2.StartingStationID = 1;
            var station2 = this.Stations.Find((s) => s.Id == 2);

            var station1 = this.Stations.Find((s) => s.Id == 1);
            station1.SetPlane(p, MoveToNextStation);
            station2.SetPlane(p2, MoveToNextStation);
            Task.Delay(new TimeSpan(0, 0, 3)).Wait();

            Assert.AreEqual(station1.CurrentPlane, p);
            Task.Delay(new TimeSpan(0, 0, 2)).Wait();
            Assert.IsNull(station1.CurrentPlane);
            Assert.AreEqual(station2.CurrentPlane, p);
        }
        [TestMethod]
        public void MovePlaneWhereNoContinue()
        {
            Plane p = new Plane(1, 2);
            var station2 = this.Stations.Find((s) => s.Id == 2);

            station2.SetPlane(p, MoveToNextStation);
            Task.Delay(new TimeSpan(0, 0, 1)).Wait();

            Assert.AreEqual(station2.CurrentPlane, p);
            Task.Delay(new TimeSpan(0, 0, 2)).Wait();

            Assert.IsNull(station2.CurrentPlane);
        }
        [TestMethod]
        public void MovePlaneWhereStationCleared()
        {
            Plane p = new Plane(1, 2);
            p.StartingStationID = 1;
            Plane p2 = new Plane(2, 4);
            p2.StartingStationID = 1;
            var station2 = this.Stations.Find((s) => s.Id == 2);

            var station1 = this.Stations.Find((s) => s.Id == 1);
            station1.SetPlane(p, MoveToNextStation);
            station2.SetPlane(p2, MoveToNextStation);
            Task.Delay(new TimeSpan(0, 0, 3)).Wait();

            Assert.AreEqual(station1.CurrentPlane, p);
            Task.Delay(new TimeSpan(0, 0, 2)).Wait();
            Assert.IsNull(station1.CurrentPlane);
            Assert.AreEqual(station2.CurrentPlane, p);
        }
        [TestMethod]
        public void DirectFlightsCorrectly()
        {
            Plane p = new Plane(1, 2);
            p.StartingStationID = 1; // is arrival
            Plane p2 = new Plane(2, 2);
            p2.StartingStationID = 6; // is departure
            var station4 = this.Stations.Find((s) => s.Id == 4);

            station4.SetPlane(p, MoveToNextStation);
            Task.Delay(new TimeSpan(0, 0, 2)).Wait(); // wait 1 seconds
            var station5 = this.Stations.Find((s) => s.Id == 5);
            Assert.AreEqual(p, station5.CurrentPlane);

            station4.SetPlane(p2, MoveToNextStation);
            Task.Delay(new TimeSpan(0, 0, 2)).Wait(); // wait 1 seconds
            Assert.AreNotEqual(p2, station5.CurrentPlane);

            Assert.IsNull(station4.CurrentPlane);
        }
        [TestMethod]
        public void DirectArrivalWhenDepartureIsBusy()
        {
            Plane p2 = new Plane(2, 5);
            p2.StartingStationID = 6; // is departure
            var station4 = this.Stations.Find((s) => s.Id == 4);

            station4.SetPlane(p2, MoveToNextStation);
            Task.Delay(new TimeSpan(0, 0, 2)).Wait(); // wait 2 seconds
            var station5 = this.Stations.Find((s) => s.Id == 5);
            Assert.AreNotEqual(p2, station5.CurrentPlane);
            Assert.AreEqual(p2, station4.CurrentPlane);
            Task.Delay(new TimeSpan(0, 0, 3)).Wait(); // wait 2 seconds
            Assert.IsNull(station4.CurrentPlane);
            Assert.IsNull(station5.CurrentPlane);
        }

        private void MoveToNextStation(object sender, System.Timers.ElapsedEventArgs e, Station station)
        {
            var route = station.CurrentPlane.StartingStationID == 1 ? station.ArrivalNextStations : station.DepartureNextStations;

            if (route.Count == 0) // if it's a terminal station, discard plane
            {
                station.CurrentPlane = null;
            }

            while (station.CurrentPlane != null)
            {
                foreach (var r in route)
                {
                    var s = this.Stations.Find((sta) => sta.Id == r);
                    if (s.CurrentPlane == null)
                    {
                        s.SetPlane(station.CurrentPlane, MoveToNextStation);

                        station.CurrentPlane = null;

                    }
                }
            }
        }
    }
}
