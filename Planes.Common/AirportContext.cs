using Planes.Common.DalModels;
using System;
using System.Data.Entity;
using System.Linq;

namespace Planes.Common
{
    public class AirportSeed : DropCreateDatabaseAlways<AirportContext>
    {
        protected override void Seed(AirportContext context)
        {
            bool IsArrival = true;

            context.Flights.Add(new Flight(1000, 100, new DateTime(2022, 3, 10, 22, 22, 11), IsArrival));
            context.Flights.Add(new Flight(1001, 101, new DateTime(2022, 3, 9, 22, 22, 11), IsArrival));
            context.Flights.Add(new Flight(1002, 102, new DateTime(2022, 3, 8, 22, 22, 11), IsArrival));
            context.Flights.Add(new Flight(1003, 103, new DateTime(2022, 3, 7, 22, 22, 11), IsArrival));
            context.Flights.Add(new Flight(1004, 104, new DateTime(2022, 3, 6, 22, 22, 11), IsArrival));
            context.Flights.Add(new Flight(1005, 105, new DateTime(2022, 3, 5, 22, 22, 11), IsArrival));


            context.Flights.Add(new Flight(2000, 200, new DateTime(2022, 3, 10, 12, 22, 11), !IsArrival));
            context.Flights.Add(new Flight(2001, 201, new DateTime(2022, 3, 9, 12, 22, 11), !IsArrival));
            context.Flights.Add(new Flight(2002, 202, new DateTime(2022, 3, 8, 12, 22, 11), !IsArrival));
            context.Flights.Add(new Flight(2003, 203, new DateTime(2022, 3, 7, 12, 22, 11), !IsArrival));
            context.Flights.Add(new Flight(2004, 204, new DateTime(2022, 3, 6, 12, 22, 11), !IsArrival));
            context.Flights.Add(new Flight(2005, 205, new DateTime(2022, 3, 5, 12, 22, 11), !IsArrival));

            base.Seed(context);
        }
    }
    public class AirportContext : DbContext
    {
        public AirportContext() : base("AirportContext")
        {
            Database.SetInitializer(new AirportSeed());
        }

        public DbSet<StationUpdates> StationRealTime { get; set; }
        public DbSet<PlanesHistory> PlanesHistory { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public IQueryable<Flight> Arrivals => Flights.Where(f => f.IsArrival);
        public IQueryable<Flight> Departures => Flights.Where(f => f.IsArrival == false);
    }
}
