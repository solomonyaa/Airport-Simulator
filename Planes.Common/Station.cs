using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Planes.Common
{
    [DataContract]
    public class Station
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<int> ArrivalNextStations { get; set; }
        [DataMember]
        public List<int> DepartureNextStations { get; set; }
        [DataMember]
        public Plane CurrentPlane { get; set; }

        public Station(int id, List<Station> arrivalNextStations = null, List<Station> depNextStations = null)
        {
            this.Id = id;
            ArrivalNextStations = new List<int>();
            if (arrivalNextStations != null)
                this.ArrivalNextStations.AddRange(arrivalNextStations.Select((x) => x.Id));

            DepartureNextStations = new List<int>();
            if (depNextStations != null)
                this.DepartureNextStations.AddRange(depNextStations.Select((x) => x.Id));

            CurrentPlane = null; // meaning station is available since it does not hold any plane
        }
        public void RemovePlane()
        {
            this.CurrentPlane = null;
        }
        public void SetPlane(Plane p)
        {
            if (this.CurrentPlane != null)
                return;
            this.CurrentPlane = p;
            if (this.CurrentPlane.StartingStationID == -1)
                this.CurrentPlane.StartingStationID = this.Id;
        }
        public void SetPlane(Plane p, Action<object, System.Timers.ElapsedEventArgs, Station> action)
        {
            if (this.CurrentPlane != null)
                return;
            this.CurrentPlane = p;
            if (this.CurrentPlane.StartingStationID == -1)
                this.CurrentPlane.StartingStationID = this.Id;
            StartTimer(action);
        }
        public void StartTimer(Action<object, System.Timers.ElapsedEventArgs, Station> action)
        {
            this.CurrentPlane.Timer = new System.Timers.Timer(CurrentPlane.TimeInterval * 1000)
            {
                AutoReset = false
            };
            this.CurrentPlane.Timer.Elapsed += (sender, e) => action(sender, e, this);
            this.CurrentPlane.Timer.Start();
        }
    }
}
