using Planes.Common;
using System.Collections.Generic;

namespace Planes.Logic.Interfaces
{
    public interface IDal
    {
        void UpdateStationStatus(Station s);
        void AddFlight(Flight f);
        IEnumerable<Flight> GetArrivals();
        IEnumerable<Flight> GetDepartures();
        void UpdatePlaneHistory(int planeId, int stationId);
    }
}
