using AvailableFlightsAggregation.AirlineApiProviders;
using AvailableFlightsAggregation.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AvailableFlightsAggregation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<AvailableFlightsHub> _availableFlightsHubContext;
        private readonly List<IAirlineApiProvider> _airlineApiProviders
            = new List<IAirlineApiProvider>();

        public HomeController(
            IHubContext<AvailableFlightsHub> availableFlightsHubContext,
            List<IAirlineApiProvider> airlineApiProviders
            )
        {
            _availableFlightsHubContext = availableFlightsHubContext;
            _airlineApiProviders = airlineApiProviders;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task PublishAvailableFlightsToClient
            (string connectionId, string signalMethodName)
        {
            await Parallel.ForEachAsync(_airlineApiProviders,
            async (airlineApiProvider, cancellationToken) =>
            {
                var airlineApiResult = 
                    await airlineApiProvider.GetAvailableFlightsAsync();
                if (airlineApiResult is null)
                {
                    return;
                }

                JsonConvert.SerializeObject
                (airlineApiResult, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                await _availableFlightsHubContext.Clients.Client(connectionId)
                    .SendAsync(signalMethodName, airlineApiResult);
            });
        }
    }
}