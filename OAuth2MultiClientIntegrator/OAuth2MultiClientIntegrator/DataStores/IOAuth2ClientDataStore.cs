using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.Gateways.GatewayDtos;

namespace OAuth2MultiClientIntegrator.DataStores
{
    internal interface IOAuth2ClientDataStore
    {
        Task<AuthenticationCodeResponse> GetAuthenticationCodeResponse(string clientId);
        Task SetAuthenticationCodeResponse(ClientCredentialOptions clientCredentialOptions,
            AuthenticationCodeResponse authenticationCodeResponse);

        Task<string> GetAuthenticationState(string clientId);
        Task SetAuthenticationState
            (ClientCredentialOptions clientCredentialOptions, string authenticationState);

        Task<AccessTokenResponse> GetAccessTokenResponse(string clientId);
        Task SetAccessTokenResponse(ClientCredentialOptions clientCredentialOptions,
            AccessTokenResponse accessTokenResponse);

        Task ClearDataStore(string clientId);
    }
}
