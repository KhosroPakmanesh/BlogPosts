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
            AuthenticationCodeResponse authenticationCodeResponse)
        {
            AuthenticationCodeResponse = authenticationCodeResponse;
            ClientName = clientCredentialOptions.ClientName;
            ClientId = clientCredentialOptions.ClientId;
        }
        public OAuth2ServerAuthInfo(
            ClientCredentialOptions clientCredentialOptions,
            string authenticationState)
        {
            AuthenticationState = authenticationState;
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
        public AuthenticationCodeResponse? AuthenticationCodeResponse { get; private set; }
        public string? AuthenticationState { get; private set; }
        public AccessTokenResponse? AccessTokenResponse { get; private set; }

        public void UpdateAuthenticationCodeResponse
            (AuthenticationCodeResponse authenticationCodeResponse)
        {
            AuthenticationCodeResponse = authenticationCodeResponse;
        }
        public void UpdateAuthenticationState(string authenticationState)
        {
            AuthenticationState = authenticationState;
        }
        public void UpdateAccessTokenResponse
            (AccessTokenResponse accessTokenResponse)
        {
            AccessTokenResponse = accessTokenResponse;
        }
    }
}
