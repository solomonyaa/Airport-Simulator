using Planes.Logic;
using Planes.Logic.FlightControlSvc;
using Planes.Logic.Interfaces;
using Planes.Logic.StationUpdaterSvc;
using System;

namespace Planes.Simulator
{
    public class ArrivalSimulator
    {
        private readonly Airport _airport;
        private int _currentFlightNum;
        public ArrivalSimulator(Airport airport)
        {
            this._airport = airport;
            _currentFlightNum = 1100;
        }
        public void AddArrival(int planeId, DateTime time = default)
        {
            _currentFlightNum++;
            if (time == default)
            {
                _airport.AddFlight(new Common.Flight(_currentFlightNum, planeId, DateTime.Now.AddMinutes(new Random().Next(1, 5)), true));
            }
            else
            {
                _airport.AddFlight(new Common.Flight(_currentFlightNum, planeId, time, true));
            }
        }
        public static void Main(string[] args)
        {
            IDal airportDal = new AirportDal();
            ((AirportDal)airportDal).InitDatabase();
            IFlightControlService flightControl = new FlightControlServiceClient();
            IStationsUpdaterService stationsUpdater = new StationsUpdaterServiceClient();

            var airport = new Airport(airportDal, flightControl, stationsUpdater);

            ArrivalSimulator simulator = new ArrivalSimulator(airport);

            for (int i = 1; i <= 20; i++)
            {
                simulator.AddArrival(i + 42, DateTime.Now.AddSeconds(i * 30));
            }

            DepartureSimulator depsimulator = new DepartureSimulator(airport);

            for (int i = 1; i <= 20; i++)
            {
                depsimulator.AddDeparture(i + 142, DateTime.Now.AddSeconds(i * 30));
            }
            Console.ReadLine();
        }
    }
}
