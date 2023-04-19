using Microsoft.AspNetCore.SignalR;

namespace FlightTicketAPIsAggregation.Hubs
{
    public class FlightTicketsHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
