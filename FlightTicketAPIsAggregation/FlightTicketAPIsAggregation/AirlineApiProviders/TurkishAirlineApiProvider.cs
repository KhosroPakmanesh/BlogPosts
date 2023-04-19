using FlightTicketAPIsAggregation.Models;

namespace FlightTicketAPIsAggregation.AirlineApiProviders
{
    public class TurkishAirlineApiProvider : IAirlineApiProvider
    {
        public async Task<AirlineApiResult> GetFlightTicketsAsync()
        {
            var random = new Random();
            var sleepInterval = random.Next(2000, 6000);
            Thread.Sleep(sleepInterval);

            return new AirlineApiResult
            {
                AirlineName = "Turkish",
                FlightTickets = new List<FlightTicket>
                {
                    new FlightTicket
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
                    new FlightTicket
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
                    new FlightTicket
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
