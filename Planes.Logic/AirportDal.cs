using Planes.Common;
using Planes.Common.DalModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Planes.Logic.Interfaces
{

    public class AirportDal : IDal
    {
        private readonly AirportContext _context;
        private readonly object thisLock = new object();

        public AirportDal()
        {
            _context = new AirportContext();
        }
        public void InitDatabase()
        {
            _context.Database.Initialize(true);
        }
        public void AddFlight(Flight f)
        {
            lock (this.thisLock)
            {
                _context.Flights.Add(f);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Flight> GetArrivals()
        {
            lock (this.thisLock)
            {
                var arrivals = _context.Arrivals.Where((f) => f.Date >= DateTime.Now).ToList();
                return arrivals;
            }
        }
        public IEnumerable<Flight> GetDepartures()
        {
            lock (this.thisLock)
            {
                return _context.Departures.Where((f) => f.Date >= DateTime.Now).ToList();
            }
        }
        public void UpdateStationStatus(Station s)
        {
            lock (this.thisLock)
            {
                var stationUpdate = new StationUpdates(s.Id, s.CurrentPlane != null); // contains a plane
                _context.StationRealTime.Add(stationUpdate);
                _context.SaveChanges();
            }
        }
        public void UpdatePlaneHistory(int planeId, int stationId)
        {
            lock (this.thisLock)
            {
                var record = _context.PlanesHistory.Where((ph) => ph.PlaneId == planeId && ph.StationId == stationId).FirstOrDefault();
                if (record == null) // if not exists
                {
                    var planeHistory = new PlanesHistory(planeId, stationId)
                    {
                        EnterTime = DateTime.Now,
                        ExitTime = DateTime.Now
                    };
                    _context.PlanesHistory.Add(planeHistory);
                }
                else
                {
                    record.ExitTime = DateTime.Now;
                }
                _context.SaveChanges();
            }
        }
    }
}
