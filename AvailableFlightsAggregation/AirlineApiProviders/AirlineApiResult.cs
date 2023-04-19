using AvailableFlightsAggregation.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace AvailableFlightsAggregation.AirlineApiProviders
{
    public class AirlineApiResult
    {
        public Guid Id { get; set; }
        public string AirlineName { get; set; }
        public List<Flight> AvailableFlights { get; set; }
            = new List<Flight>();
    }
}