using Microsoft.Extensions.Options;
using MVCWebApplication.AppConfigurations.OAuth2MultiClientIntegrator.Options;
using OAuth2MultiClientIntegrator.Connectors.Configurations;
using OAuth2MultiClientIntegrator.Connectors.Dtos;


namespace MVCWebApplication.AppConfigurations.OAuth2MultiClientIntegrator
{
    public static class ConfigureOAuth2MultiClientIntegratorClients
    {
        public static IServiceCollection AddOAuth2MultiClientIntegratorClients
            (this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<OAuth2IntegrationsOptions>
                (options => configuration.GetSection
                ("OAuth2Integrations").Bind(options));

            var options = services.BuildServiceProvider()
                .GetRequiredService<IOptions<OAuth2IntegrationsOptions>>();

            services.AddOAuth2MultiClientIntegratorClient(
                new OAuth2Client
                (
                    new ClientCredentialOptions
                    {
                        ClientName = "ARegularClient",
                        ClientId = options.Value.ARegularClientSettings.ClientSettings.ClientId,
                        ClientSecret = options.Value.ARegularClientSettings.ClientSettings.ClientSecret
                    },
                    new AuthenticationCodeOptions
                    {
                        BaseAuthenticationUri = "BaseAuthenticationUri",
                        TargetUri = options.Value.ARegularClientSettings.ClientSettings.TargetUri,
                        Scope = "Scope",
                        NonestandardSettings = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("account_type","service")
                        },
                        StaticExpirationValue = 600,
                    },
                    new AccessTokenOptions
                    {
                        BaseAccessTokenUri = "BaseAccessTokenUri"
                    }
                )
            );
            services.AddOAuth2MultiClientIntegratorClient(
                new OAuth2Client
                (
                    new ClientCredentialOptions
                    {
                        ClientName = "GoogleClient",
                        ClientId = options.Value.FacebookSettings.ClientSettings.ClientId,
                        ClientSecret = options.Value.FacebookSettings.ClientSettings.ClientSecret
                    },
                    new AuthenticationCodeOptions
                    {
                        BaseAuthenticationUri = "https://accounts.google.com/o/oauth2/v2/auth",
                        TargetUri = options.Value.FacebookSettings.ClientSettings.TargetUri,
                        Scope = "https://www.googleapis.com/auth/business.manage",
                        StaticExpirationValue = 60
                    },
                    new AccessTokenOptions
                    {
                        BaseAccessTokenUri = "https://oauth2.googleapis.com/token"
                    }
                )
            );
            services.AddOAuth2MultiClientIntegratorClient(
                new OAuth2Client
                (
                    new ClientCredentialOptions
                    {
                        ClientName = "FacebookClient",
                        ClientId = options.Value.GoogleSettings.ClientSettings.ClientId,
                        ClientSecret = options.Value.GoogleSettings.ClientSettings.ClientSecret
                    },
                    new AuthenticationCodeOptions
                    {
                        BaseAuthenticationUri = "https://www.facebook.com/v18.0/dialog/oauth",
                        TargetUri = options.Value.GoogleSettings.ClientSettings.TargetUri,
                        Scope = "public_profile,email",
                        StaticExpirationValue = 60,
                        NonestandardSettings = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("config_id","SomeValue")
                        }
                    },
                    new AccessTokenOptions
                    {
                        BaseAccessTokenUri = "https://graph.facebook.com/v18.0/oauth/access_token",
                        StaticExpirationValue = 5184000,
                        CanRenewAfterExpiration = false
                    },
                    new RefreshTokenOptions
                    {
                        GrantType = "fb_exchange_token",
                        StaticExpirationValue = 5184000,
                        CanRenewAfterExpiration = false,
                        StandardAlternativeNamePairs = new List<Tuple<string, string>>
                        {
                            new Tuple<string, string>( "refresh_token","fb_exchange_token")
                        },
                        NonestandardSettings = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("set_token_expires_in_60_days","true")
                        }
                    }
                )
            );

            return services;
        }
    }
}
