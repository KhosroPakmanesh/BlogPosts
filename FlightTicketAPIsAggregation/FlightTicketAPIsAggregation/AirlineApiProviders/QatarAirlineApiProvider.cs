using FlightTicketAPIsAggregation.Models;

namespace FlightTicketAPIsAggregation.AirlineApiProviders
{
    public class QatarAirlineApiProvider : IAirlineApiProvider
    {
        public async Task<AirlineApiResult> GetFlightTicketsAsync()
        {
            var random = new Random();
            var sleepInterval = random.Next(2000, 6000);
            Thread.Sleep(sleepInterval);

            return new AirlineApiResult
            {
                AirlineName = "Qatar",
                FlightTickets = new List<FlightTicket>
                {
                    new FlightTicket
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
                    new FlightTicket
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
