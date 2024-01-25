using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LogEnabledHttpClient.LoggerSettings
{
    internal interface ILoggerSetting
    {
        void ApplySettings(IServiceCollection serviceCollection);
    }
}
