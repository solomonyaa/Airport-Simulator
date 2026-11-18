using System;
using System.ComponentModel.DataAnnotations;

namespace Planes.Common.DalModels
{
    public class StationUpdates
    {
        [Key]
        public int UpdateId { get; set; }
        public DateTime UpdateTime { get; set; }
        public int StationId { get; set; }
        public bool HasPlane { get; set; }

        public StationUpdates()
        {
        }
        public StationUpdates(int stationId, bool hasPlane)
        {
            this.UpdateTime = DateTime.Now;
            StationId = stationId;
            HasPlane = hasPlane;
        }
    }
}
