using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Planes.Common
{
    [DataContract]
    public class Flight
    {
        [DataMember]
        [Key]
        public int FlightId { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        [NotMapped]
        public Plane Plane { get; set; }
        [DataMember]
        public bool IsArrival { get; set; }
        public Flight()
        {
        }
        public Flight(int id, int planeId, DateTime date, bool isArrival, int timeout = -1)
        {
            this.FlightId = id;
            this.Date = date;
            this.Plane = new Plane(planeId, timeout == -1 ? new Random().Next(10, 15) : timeout)
            {
                StartingStationID = isArrival ? 1 : new Random().Next(6, 8)
            };
            this.IsArrival = isArrival;
        }
    }
}
