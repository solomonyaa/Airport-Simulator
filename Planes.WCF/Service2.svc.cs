using Planes.Common;
using System.Collections.Generic;
using System.Linq;

namespace Planes.WCF
{
    public class StationUpdaterService : IStationsUpdaterService
    {
        public static List<Station> Stations = new List<Station>();
        public object thisLock = new object();
        public static bool initOnce = true;

        public StationUpdaterService()
        {
            if (initOnce)
            {
                SetStations();
                initOnce = false;
            }
        }
        public List<Station> GetStations()
        {
            // Lock keyword ensures that one thread is executing a piece of code at one time.
            // The lock keyword ensures that one thread does not enter a critical section of code while another thread is in that critical section.
            lock (thisLock)
            {
                return Stations;
            }
        }
        public Station GetStation(int id)
        {
            return Stations.Where((f) => f.Id == id).First();
        }
        public void SetPlaneInStation(int id, Plane p)
        {
            lock (thisLock)
            {
                var stationIdx = Stations.FindIndex((s) => s.Id == id);

                if (stationIdx != -1)
                {
                    if (p == null)
                        Stations[stationIdx].RemovePlane();
                    else
                        Stations[stationIdx].SetPlane(p);
                }
            }
        }
        public bool IsStationAvailable(int id)
        {
            lock (thisLock)
            {
                var station = Stations.FirstOrDefault((s) => s.Id == id);
                return station != null && station.CurrentPlane == null;
            }
        }
        public void SetStations()
        {
            // build routes by design showed in excercise
            var station8 = new Station(8);
            var station7 = new Station(7, null, new List<Station> { station8 });
            var station6 = new Station(6, null, new List<Station> { station8 });
            var station5 = new Station(5, new List<Station> { station7, station6 });
            var station4 = new Station(4, new List<Station> { station5 });
            var station3 = new Station(3, new List<Station> { station4 });
            var station2 = new Station(2, new List<Station> { station3 });
            var station1 = new Station(1, new List<Station> { station2 });
            station8.DepartureNextStations.Add(4);

            Stations.AddRange(new List<Station>
                            { station1, station2, station3, station4, station5, station6, station7, station8 });
        }
    }
}
