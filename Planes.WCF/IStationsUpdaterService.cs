using Planes.Common;
using System.Collections.Generic;
using System.ServiceModel;

namespace Planes.WCF
{
    [ServiceContract]
    public interface IStationsUpdaterService
    {
        [OperationContract]
        List<Station> GetStations();
        [OperationContract]
        Station GetStation(int id);

        [OperationContract]
        void SetStations();
        [OperationContract]
        void SetPlaneInStation(int stationId, Plane p);
        [OperationContract]
        bool IsStationAvailable(int id);
    }
}
