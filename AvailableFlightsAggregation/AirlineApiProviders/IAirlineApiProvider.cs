namespace AvailableFlightsAggregation.AirlineApiProviders
{
    public interface IAirlineApiProvider
    {
        public Task<AirlineApiResult> GetAvailableFlightsAsync();
    }
}
