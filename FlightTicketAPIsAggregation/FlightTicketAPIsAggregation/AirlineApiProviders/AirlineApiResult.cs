using FlightTicketAPIsAggregation.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace FlightTicketAPIsAggregation.AirlineApiProviders
{
    public class AirlineApiResult
    {
        public Guid Id { get; set; }
        public string AirlineName { get; set; }
        public List<FlightTicket> FlightTickets { get; set; }
            = new List<FlightTicket>();
    }
}