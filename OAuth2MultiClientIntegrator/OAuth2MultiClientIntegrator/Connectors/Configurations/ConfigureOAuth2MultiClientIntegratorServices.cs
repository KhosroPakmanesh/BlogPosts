using Microsoft.EntityFrameworkCore;
using OAuth2MultiClientIntegrator.Connectors.ActionFilters;
using OAuth2MultiClientIntegrator.DataStores;
using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore;
using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.EFDbContext;
using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.Repository;
using OAuth2MultiClientIntegrator.DateTimerProvider;
using OAuth2MultiClientIntegrator.Gateways;

namespace OAuth2MultiClientIntegrator.Connectors.Configurations
{
    internal static class ConfigureOAuth2MultiClientIntegratorServices
    {
        public static IServiceCollection AddOAuth2ClientServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IOAuth2ClientGateway, DefaultOAuth2ClientGateway>();
            services.AddScoped<IOAuth2ClientDataStore, DefaultDbBasedClientDataStore>();
            services.AddScoped<IOAuth2AccessTokenProvider, DefaultOAuth2AccessTokenProvider>();
            services.AddScoped<IOAuth2TargetUriGenerator, DefaultOAuth2TargetUriGenerator>();
            services.AddScoped<IOAuth2AuthorizationUriGenerator, DefaultOAuth2AuthorizationUriGenerator>();

            services.AddDbContext<OAuth2ServerAuthInfoDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("oauth2-multi-client-integrator-db");
                options.UseSqlServer(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });
            services.AddScoped<IOAuth2ServerAuthInfoRepository, OAuth2ServerAuthInfoRepository>();

            services.AddMvc().AddMvcOptions(options =>
            {
                options.Filters.Add(
                    new OAuth2AuthorizationRedirectSignalExceptionFilter());
            });

            return services;
        }
    }
}
