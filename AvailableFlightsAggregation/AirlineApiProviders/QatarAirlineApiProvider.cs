using AvailableFlightsAggregation.Models;

namespace AvailableFlightsAggregation.AirlineApiProviders
{
    public class QatarAirlineApiProvider : IAirlineApiProvider
    {
        public async Task<AirlineApiResult> GetAvailableFlightsAsync()
        {
            var random = new Random();
            var sleepInterval = random.Next(2000, 6000);
            Thread.Sleep(sleepInterval);

            return new AirlineApiResult
            {
                AirlineName = "Qatar",
                AvailableFlights = new List<Flight>
                {
                    new Flight
                    {
                        FlightNumber ="JKL123",
                        BoardingDateTime =DateTime.Now.AddHours(3),
                        SourceCity= "Stockholm",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 5,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=60
                    },
                    new Flight
                    {
                        FlightNumber ="MNO456",
                        BoardingDateTime =DateTime.Now.AddHours(5),
                        SourceCity= "Stockholm",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 23,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=65
                    }
                }
            };
        }
    }
}
