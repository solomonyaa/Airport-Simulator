using Planes.Common;
using System.Collections.Generic;

namespace Planes.WCF
{
    public class FlightControlService : IFlightControlService
    {
        public static List<Flight> Arrivals = new List<Flight>();
        public static List<Flight> Departures = new List<Flight>();

        public void ClearFlights()
        {
            Arrivals = new List<Flight>();
            Departures = new List<Flight>();
        }
        public void AddFlight(Flight f)
        {
            if (f.IsArrival)
                Arrivals.Add(f);
            else
            {
                Departures.Add(f);
            }
        }

        public IEnumerable<Flight> GetAllFlights(bool isArrival)
        {
            return isArrival ? Arrivals : Departures;
        }
        public bool RemoveFlight(Flight f)
        {
            return f.IsArrival ? Arrivals.Remove(f) : Departures.Remove(f);
        }

        public void AddFlights(IEnumerable<Flight> flights)
        {
            foreach (var f in flights)
            {
                AddFlight(f);
            }
        }
    }
}
