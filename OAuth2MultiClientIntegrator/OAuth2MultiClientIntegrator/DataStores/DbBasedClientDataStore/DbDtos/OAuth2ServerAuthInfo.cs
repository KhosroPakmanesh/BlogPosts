using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.Gateways.GatewayDtos;

namespace OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.DbDtos
{
    internal sealed class OAuth2ServerAuthInfo
    {
        private OAuth2ServerAuthInfo()
        {

        }

        public OAuth2ServerAuthInfo
            (ClientCredentialOptions clientCredentialOptions,
            AuthorizationCodeResponse authorizationCodeResponse)
        {
            AuthorizationCodeResponse = authorizationCodeResponse;
            ClientName = clientCredentialOptions.ClientName;
            ClientId = clientCredentialOptions.ClientId;
        }
        public OAuth2ServerAuthInfo(
            ClientCredentialOptions clientCredentialOptions,
            string authorizationState)
        {
            AuthorizationState = authorizationState;
            ClientName = clientCredentialOptions.ClientName;
            ClientId = clientCredentialOptions.ClientId;
        }
        public OAuth2ServerAuthInfo(
            ClientCredentialOptions clientCredentialOptions,
            AccessTokenResponse accessTokenResponse)
        {
            AccessTokenResponse = accessTokenResponse;
            ClientName = clientCredentialOptions.ClientName;
            ClientId = clientCredentialOptions.ClientId;
        }

        public Guid Id { get; private set; }
        public string ClientName { get; private set; }
        public string ClientId { get; private set; }
        public AuthorizationCodeResponse? AuthorizationCodeResponse { get; private set; }
        public string? AuthorizationState { get; private set; }
        public AccessTokenResponse? AccessTokenResponse { get; private set; }

        public void UpdateAuthorizationCodeResponse
            (AuthorizationCodeResponse authorizationCodeResponse)
        {
            AuthorizationCodeResponse = authorizationCodeResponse;
        }
        public void UpdateAuthorizationState(string authorizationState)
        {
            AuthorizationState = authorizationState;
        }
        public void UpdateAccessTokenResponse
            (AccessTokenResponse accessTokenResponse)
        {
            AccessTokenResponse = accessTokenResponse;
        }
    }
}
