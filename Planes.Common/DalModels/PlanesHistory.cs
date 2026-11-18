using System;
using System.ComponentModel.DataAnnotations;

namespace Planes.Common.DalModels
{
    public class PlanesHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public int PlaneId { get; set; }
        public int StationId { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime ExitTime { get; set; }

        public PlanesHistory()
        { 
        }
        public PlanesHistory(int planeId, int stationId)
        {
            this.PlaneId = planeId;
            this.StationId = stationId;
        }
    }
}
