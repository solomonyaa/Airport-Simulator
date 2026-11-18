using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Timers;

namespace Planes.Common
{
    [DataContract]
    public class Plane
    {
        [DataMember]
        [Key]
        public int Id { get; set; }
        [DataMember]
        public int TimeInterval { get; set; }

        public Timer Timer;
        [DataMember]
        public int StartingStationID { get; set; }
        public Plane()
        {
            this.Timer = new Timer
            {
                AutoReset = false
            };
            StartingStationID = -1;
        }
        public Plane(int id, int timeInterval)
        {
            this.Id = id;
            this.TimeInterval = timeInterval;
            this.Timer = new Timer(TimeInterval * 1000)
            {
                AutoReset = false
            };
            StartingStationID = -1;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Plane))
                return false;
            var other_plane = obj as Plane;
            return this.Id == other_plane.Id;
        }
        public override string ToString()
        {
            return $"Plane ID: {this.Id}";
        }
    }
}
