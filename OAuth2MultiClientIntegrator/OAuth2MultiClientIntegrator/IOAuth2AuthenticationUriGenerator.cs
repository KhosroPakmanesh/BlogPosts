namespace OAuth2MultiClientIntegrator
{
    internal interface IOAuth2AuthenticationUriGenerator
    {
        Task<string> GenerateAuthenticationUri(string clientId, string authenticationRedirectUri);
    }
}
