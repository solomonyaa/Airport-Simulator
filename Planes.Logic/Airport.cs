using Planes.Common;
using Planes.Logic.FlightControlSvc;
using Planes.Logic.Interfaces;
using Planes.Logic.StationUpdaterSvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Planes.Logic
{
    public class Airport
    {
        private readonly IFlightControlService _flightService;
        private readonly IStationsUpdaterService _stationUpdater;
        private readonly IDal _dal;
        public Airport(IDal dal, IFlightControlService flightControl, IStationsUpdaterService stationsUpdater)
        {
            this._flightService = flightControl;
            this._stationUpdater = stationsUpdater;
            this._dal = dal;
            Task.Factory.StartNew(CheckArrivalsAndDepartures);
            SetFlightsFromRepository();
        }
        public void AddFlight(Flight f)
        {
            this._flightService.AddFlight(f);
            this._dal.AddFlight(f);
        }

        private void MoveToNextStation(object sender, System.Timers.ElapsedEventArgs e, Station station)
        {
            var route = station.CurrentPlane.StartingStationID == 1 ? station.ArrivalNextStations : station.DepartureNextStations;

            if (route.Count == 0) // if it's a terminal station, discard plane.
            {
                _dal.UpdatePlaneHistory(station.CurrentPlane.Id, station.Id);
                _stationUpdater.SetPlaneInStation(station.Id, null);
                station.RemovePlane();
                _dal.UpdateStationStatus(station);
                return;
            }

            while (station.CurrentPlane != null)
            {
                var l = route.OrderBy(x => new Random().Next()).ToList();
                foreach (var r in l)
                {
                    int index = new Random().Next(route.Count);

                    var s = _stationUpdater.GetStation(route[index]);
                    if (s.CurrentPlane == null)
                    {
                        s.SetPlane(station.CurrentPlane, MoveToNextStation);
                        _dal.UpdatePlaneHistory(station.CurrentPlane.Id, s.Id);
                        _dal.UpdatePlaneHistory(station.CurrentPlane.Id, station.Id);

                        _stationUpdater.SetPlaneInStation(station.Id, null);
                        _stationUpdater.SetPlaneInStation(s.Id, station.CurrentPlane);
                        station.RemovePlane();

                        _dal.UpdateStationStatus(s);
                        _dal.UpdateStationStatus(station);
                        break;
                    }
                }
            }
        }
        public List<Station> GetCurrentStations()
        {
            return _stationUpdater.GetStations();
        }
        public bool EqualsUpToSeconds(DateTime dt1, DateTime dt2)
        {
            return dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day &&
                   dt1.Hour == dt2.Hour && dt1.Minute == dt2.Minute && dt1.Second == dt2.Second;
        }
        public Task CheckArrivalsAndDepartures()
        {
            while (true)
            {
                var arrivals = _dal.GetArrivals().ToList();
                arrivals = arrivals.Where((flight) => EqualsUpToSeconds(flight.Date, DateTime.Now)).ToList();
                foreach (var arrival in arrivals)
                {
                    if (_stationUpdater.IsStationAvailable(1))
                    {
                        _stationUpdater.SetPlaneInStation(1, arrival.Plane);
                        var currentStation = _stationUpdater.GetStation(1);
                        _dal.UpdateStationStatus(currentStation);
                        _dal.UpdatePlaneHistory(arrival.Plane.Id, currentStation.Id);
                        currentStation.StartTimer(MoveToNextStation);
                    }
                }
                var departures = _dal.GetDepartures().ToList().Where((flight) => EqualsUpToSeconds(flight.Date, DateTime.Now));
                foreach (var departure in departures)
                {
                    if (_stationUpdater.IsStationAvailable(6))
                    {
                        _stationUpdater.SetPlaneInStation(6, departure.Plane);
                        var currentStation = _stationUpdater.GetStation(6);
                        _dal.UpdateStationStatus(currentStation);
                        _dal.UpdatePlaneHistory(departure.Plane.Id, currentStation.Id);
                        currentStation.StartTimer(MoveToNextStation);

                    }
                    else if (_stationUpdater.IsStationAvailable(7))
                    {
                        _stationUpdater.SetPlaneInStation(7, departure.Plane);
                        var currentStation = _stationUpdater.GetStation(7);
                        _dal.UpdateStationStatus(currentStation);
                        _dal.UpdatePlaneHistory(departure.Plane.Id, currentStation.Id);
                        currentStation.StartTimer(MoveToNextStation);
                    }
                }
                Thread.Sleep(1000);
            }
        }
        public void SetFlightsFromRepository()
        {
            var arrivals = _dal.GetArrivals().ToList();
            var departures = _dal.GetDepartures().ToList();
            _flightService.AddFlights(arrivals);
            _flightService.AddFlights(departures);
        }
    }
}
