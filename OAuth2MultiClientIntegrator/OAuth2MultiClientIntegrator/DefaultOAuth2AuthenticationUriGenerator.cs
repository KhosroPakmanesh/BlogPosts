using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.DataStores;
using OAuth2MultiClientIntegrator.Exceptions;

namespace OAuth2MultiClientIntegrator
{
    internal sealed class DefaultOAuth2AuthenticationUriGenerator : IOAuth2AuthenticationUriGenerator
    {
        private readonly IOAuth2ClientDataStore _oAuth2ClientDataStore;
        private readonly List<OAuth2Client> _oAuth2Clients;

        public DefaultOAuth2AuthenticationUriGenerator(
            IOAuth2ClientDataStore oAuth2ClientDataStore,
            List<OAuth2Client> oAuth2Clients)
        {
            _oAuth2ClientDataStore = oAuth2ClientDataStore;
            _oAuth2Clients = oAuth2Clients;
        }

        public async Task<string> GenerateAuthenticationUri
            (string clientId, string authenticationRedirectUri)
        {
            var oAuth2Client = _oAuth2Clients.FirstOrDefault
                (t => t.ClientCredentialOptions.ClientId == clientId);
            if (oAuth2Client == null)
            {
                throw new OAuth2MultiClientIntegratorFailureException
                    ("No suitable OAuthClient found.");
            }

            var authenticationState = $"{Guid.NewGuid()},{clientId}";
            await _oAuth2ClientDataStore.SetAuthenticationState
                (oAuth2Client.ClientCredentialOptions, authenticationState);

            var fullAuthenticationUri = oAuth2Client.AuthenticationCodeOptions
                .GenerateFullAuthenticationUri(clientId, authenticationState,
                    authenticationRedirectUri);
            return fullAuthenticationUri;
        }
    }
}
