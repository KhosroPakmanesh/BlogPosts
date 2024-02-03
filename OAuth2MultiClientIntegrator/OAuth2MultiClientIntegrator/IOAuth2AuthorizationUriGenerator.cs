namespace OAuth2MultiClientIntegrator
{
    internal interface IOAuth2AuthorizationUriGenerator
    {
        Task<string> GenerateAuthorizationUri(string clientId, string authorizationRedirectUri);
    }
}
