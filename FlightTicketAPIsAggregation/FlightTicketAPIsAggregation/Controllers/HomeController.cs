using FlightTicketAPIsAggregation.AirlineApiProviders;
using FlightTicketAPIsAggregation.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlightTicketAPIsAggregation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<FlightTicketsHub> _flightTicketsHubContext;
        private readonly List<IAirlineApiProvider> _airlineApiProviders
            = new List<IAirlineApiProvider>();

        public HomeController(
            IHubContext<FlightTicketsHub> flightTicketsHubContext,
            List<IAirlineApiProvider> airlineApiProviders
            )
        {
            _flightTicketsHubContext = flightTicketsHubContext;
            _airlineApiProviders = airlineApiProviders;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task PublishFlightTicketsToClient
            (string connectionId, string signalMethodName)
        {
            await Parallel.ForEachAsync(_airlineApiProviders,
                async (airlineApiProvider, cancellationToken) =>
                {
                    var airlineApiResult = 
                        await airlineApiProvider.GetFlightTicketsAsync();
                    if (airlineApiResult is null)
                    {
                        return;
                    }

                    JsonConvert.SerializeObject
                    (airlineApiResult, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                    await _flightTicketsHubContext.Clients.Client(connectionId)
                        .SendAsync(signalMethodName, airlineApiResult);
                });
        }
    }
}