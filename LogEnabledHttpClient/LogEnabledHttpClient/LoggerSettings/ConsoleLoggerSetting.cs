using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LogEnabledHttpClient.LoggerSettings
{
    internal sealed class ConsoleLoggerSetting : ILoggerSetting
    {
        public void ApplySettings(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging((loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
            });
        }
    }
}
