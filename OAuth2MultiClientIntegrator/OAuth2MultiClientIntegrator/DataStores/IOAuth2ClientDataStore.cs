using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.Gateways.GatewayDtos;

namespace OAuth2MultiClientIntegrator.DataStores
{
    internal interface IOAuth2ClientDataStore
    {
        Task<AuthorizationCodeResponse> GetAuthorizationCodeResponse(string clientId);
        Task SetAuthorizationCodeResponse(ClientCredentialOptions clientCredentialOptions,
            AuthorizationCodeResponse authorizationCodeResponse);

        Task<string> GetAuthorizationState(string clientId);
        Task SetAuthorizationState
            (ClientCredentialOptions clientCredentialOptions, string authorizationState);

        Task<AccessTokenResponse> GetAccessTokenResponse(string clientId);
        Task SetAccessTokenResponse(ClientCredentialOptions clientCredentialOptions,
            AccessTokenResponse accessTokenResponse);

        Task ClearDataStore(string clientId);
    }
}
