using OAuth2MultiClientIntegrator.Connectors.Dtos;

namespace OAuth2MultiClientIntegrator.Connectors.Configurations
{
    public static class ConfigureOAuth2MultiClientIntegrator
    {
        public static IServiceCollection AddOAuth2MultiClientIntegratorServices
            (this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOAuth2ClientServices(configuration);

            return services;
        }
        public static IServiceCollection AddOAuth2MultiClientIntegratorClient
            (this IServiceCollection services, OAuth2Client oAuth2Client)
        {
            var oAuth2Clients = services.
                BuildServiceProvider().GetService<List<OAuth2Client>>();

            if (oAuth2Clients == null)
            {
                oAuth2Clients = new List<OAuth2Client>() { oAuth2Client };
                services.AddSingleton(oAuth2Clients);
            }
            else
            {
                oAuth2Clients.Add(oAuth2Client);
                services.AddSingleton(oAuth2Clients);
            }

            return services;
        }
        public static IApplicationBuilder UseOAuth2MultiClientIntegrator
            (this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .RegisterOAuth2IdentityServerCallbackEndpoint();

            return applicationBuilder;
        }
    }
}
