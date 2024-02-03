using LogEnabledHttpClient.DelegatingHandlers;
using LogEnabledHttpClient.LoggerSettings;

namespace LogEnabledHttpClient.Configurations
{
    public static class ConfigureLogEnabledHttpClient
    {
        public static IServiceCollection AddLogEnabledHttpClient
            (this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<LoggingDelegatingHandler>();
            serviceCollection.AddHttpClient("LogEnabledClient")
                .AddHttpMessageHandler<LoggingDelegatingHandler>();

            #if DEBUG
            {
                serviceCollection.AddSingleton<ILoggerSetting,
                    ConsoleLoggerSetting>();
            }
            #else
            {
                serviceCollection.AddSingleton<ILoggerSetting,
                    ApplicationInsightsLoggerSetting>();
            }            
            #endif

            var loggerSetting = serviceCollection.BuildServiceProvider()
                .GetRequiredService<ILoggerSetting>();

            loggerSetting.ApplySettings(serviceCollection);

            return serviceCollection;
        }
    }
}
