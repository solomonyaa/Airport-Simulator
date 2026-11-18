using Planes.Common;
using System.Collections.Generic;
using System.ServiceModel;

namespace Planes.WCF
{
    [ServiceContract]
    public interface IFlightControlService
    {
        [OperationContract]
        void ClearFlights();
        [OperationContract]
        void AddFlight(Flight f);
        [OperationContract]
        void AddFlights(IEnumerable<Flight> flights);
        [OperationContract]
        bool RemoveFlight(Flight f);
        [OperationContract]
        IEnumerable<Flight> GetAllFlights(bool isArrival);
    }
}
