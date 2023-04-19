using Microsoft.AspNetCore.SignalR;

namespace AvailableFlightsAggregation.Hubs
{
    public class AvailableFlightsHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
