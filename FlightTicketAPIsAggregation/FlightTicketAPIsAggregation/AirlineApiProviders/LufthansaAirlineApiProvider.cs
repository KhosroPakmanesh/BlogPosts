using FlightTicketAPIsAggregation.Models;

namespace FlightTicketAPIsAggregation.AirlineApiProviders
{
    public class LufthansaAirlineApiProvider : IAirlineApiProvider
    {
        public async Task<AirlineApiResult> GetFlightTicketsAsync()
        {
            var random = new Random();
            var sleepInterval = random.Next(2000, 6000);
            Thread.Sleep(sleepInterval);

            return new AirlineApiResult
            {
                AirlineName = "Lufthansa",
                FlightTickets= new List<FlightTicket>
                {
                    new FlightTicket
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
                                        new FlightTicket
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
                    new FlightTicket
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
                    new FlightTicket
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
