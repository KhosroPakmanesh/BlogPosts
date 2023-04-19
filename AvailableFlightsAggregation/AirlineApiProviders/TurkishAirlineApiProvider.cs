using AvailableFlightsAggregation.Models;

namespace AvailableFlightsAggregation.AirlineApiProviders
{
    public class TurkishAirlineApiProvider : IAirlineApiProvider
    {
        public async Task<AirlineApiResult> GetAvailableFlightsAsync()
        {
            var random = new Random();
            var sleepInterval = random.Next(2000, 6000);
            Thread.Sleep(sleepInterval);

            return new AirlineApiResult
            {
                AirlineName = "Turkish",
                AvailableFlights = new List<Flight>
                {
                    new Flight
                    {
                        FlightNumber ="ABC123",
                        BoardingDateTime =DateTime.Now,
                        SourceCity= "Stockholm ",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 35,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=50
                    },
                    new Flight
                    {
                        FlightNumber ="DEF456",
                        BoardingDateTime =DateTime.Now.AddHours(2),
                        SourceCity= "Stockholm",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 12,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=45
                    },
                    new Flight
                    {
                        FlightNumber ="GHI789",
                        BoardingDateTime =DateTime.Now.AddHours(4),
                        SourceCity= "Stockholm",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 8,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=51
                    }
                }
            };
        }
    }
}
