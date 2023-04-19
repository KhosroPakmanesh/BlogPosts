namespace FlightTicketAPIsAggregation.AirlineApiProviders
{
    public interface IAirlineApiProvider
    {
        public Task<AirlineApiResult> GetFlightTicketsAsync();
    }
}
