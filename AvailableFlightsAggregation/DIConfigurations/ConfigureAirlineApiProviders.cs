using AvailableFlightsAggregation.AirlineApiProviders;

namespace AvailableFlightsAggregation.DIConfigurations;

public static class ConfigureAirlineApiProviders
{
    public static IServiceCollection AddAirlineApiProviders
        (this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(
        new List<IAirlineApiProvider>
        {
            new TurkishAirlineApiProvider(),
            new LufthansaAirlineApiProvider(),
            new QatarAirlineApiProvider(),
        });

        return serviceCollection;
    }
}
