using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace LogEnabledHttpClient.LoggerSettings
{
    internal sealed class ApplicationInsightsLoggerSetting : ILoggerSetting
    {
        public void ApplySettings(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging((loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
                loggingBuilder.AddAzureWebAppDiagnostics();
            });

            serviceCollection.Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "your-project-name-diagnostics-";
                options.FileSizeLimit = 50 * 1024;
                options.RetainedFileCountLimit = 5;
            });
        }
    }
}
