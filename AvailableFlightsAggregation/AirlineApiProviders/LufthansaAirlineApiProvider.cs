using AvailableFlightsAggregation.Models;

namespace AvailableFlightsAggregation.AirlineApiProviders
{
    public class LufthansaAirlineApiProvider : IAirlineApiProvider
    {
        public async Task<AirlineApiResult> GetAvailableFlightsAsync()
        {
            var random = new Random();
            var sleepInterval = random.Next(2000, 6000);
            Thread.Sleep(sleepInterval);

            return new AirlineApiResult
            {
                AirlineName = "Lufthansa",
                AvailableFlights= new List<Flight>
                {
                    new Flight
                    {
                        FlightNumber ="PQR123",
                        BoardingDateTime =DateTime.Now.AddHours(3),
                        SourceCity= "Stockholm",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 35,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=40
                    },
                                        new Flight
                    {
                        FlightNumber ="STU456",
                        BoardingDateTime =DateTime.Now.AddHours(4),
                        SourceCity= "Stockholm",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 8,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=45
                    },
                    new Flight
                    {
                        FlightNumber ="VWX789",
                        BoardingDateTime =DateTime.Now.AddHours(7),
                        SourceCity= "Stockholm",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 13,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=40
                    },
                    new Flight
                    {
                        FlightNumber ="YZA123",
                        BoardingDateTime =DateTime.Now.AddHours(12),
                        SourceCity= "Stockholm",
                        DestinationCity = "Hamburg",
                        AvailableSeatNumbers= 45,
                        AllowableCargoWeight= 21,
                        SourceAirport="Stockholm Airport",
                        DestinationAirport = "Hamburg Airport",
                        Price=50
                    }
                }
            };
        }
    }
}
