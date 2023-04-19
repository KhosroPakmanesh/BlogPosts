namespace AvailableFlightsAggregation.Models
{
    public class Flight
    {
        public string FlightNumber { get; set; }
        public DateTime BoardingDateTime { get; set; }

        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }

        public byte AvailableSeatNumbers { get; set; }
        public byte AllowableCargoWeight { get; set; }

        public string SourceAirport { get; set; }
        public string DestinationAirport { get; set; }

        public decimal Price { get; set; }
    }
}
