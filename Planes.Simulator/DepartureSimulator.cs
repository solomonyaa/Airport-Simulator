using Planes.Logic;
using System;

namespace Planes.Simulator
{
    public class DepartureSimulator
    {
        private readonly Airport _airport;
        private int _currentFlightNum;
        public DepartureSimulator(Airport airport)
        {
            this._airport = airport;
            _currentFlightNum = 2100;
        }
        public void AddDeparture(int planeId, DateTime time = default)
        {
            _currentFlightNum++;
            if (time == default)
            {
                _airport.AddFlight(new Common.Flight(_currentFlightNum, planeId, DateTime.Now.AddMinutes(new Random().Next(2, 8)), false));
            }
            else
            {
                _airport.AddFlight(new Common.Flight(_currentFlightNum, planeId, time, false));
            }
        }
    }
}
