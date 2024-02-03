using OAuth2MultiClientIntegrator.Models;

namespace OAuth2MultiClientIntegrator.Connectors.Configurations
{
    internal static class ConfigureOAuthMultiClientIntegratorAppSettings
    {
        public static IApplicationBuilder RegisterOAuth2IdentityServerCallbackEndpoint
         (this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapGet(
                OAuth2ClientManagerSettings.DefaultOAuth2RedirectUri,
                async httpContext =>
                {
                    using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

                    var oauth2TargetUriGenerator = serviceScope.ServiceProvider
                        .GetRequiredService<IOAuth2TargetUriGenerator>();

                    string authorizationCode = httpContext.Request.Query["code"]!;
                    string authorizationState = httpContext.Request.Query["state"]!;

                    string targetUri = await oauth2TargetUriGenerator
                        .GenerateTargetUri(authorizationCode, authorizationState);

                    httpContext.Response.Redirect(targetUri);
                });
            });

            return applicationBuilder;
        }
    }
}
