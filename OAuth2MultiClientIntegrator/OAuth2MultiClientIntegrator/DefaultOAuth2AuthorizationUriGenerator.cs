using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.DataStores;
using OAuth2MultiClientIntegrator.Exceptions;

namespace OAuth2MultiClientIntegrator
{
    internal sealed class DefaultOAuth2AuthorizationUriGenerator : IOAuth2AuthorizationUriGenerator
    {
        private readonly IOAuth2ClientDataStore _oAuth2ClientDataStore;
        private readonly List<OAuth2Client> _oAuth2Clients;

        public DefaultOAuth2AuthorizationUriGenerator(
            IOAuth2ClientDataStore oAuth2ClientDataStore,
            List<OAuth2Client> oAuth2Clients)
        {
            _oAuth2ClientDataStore = oAuth2ClientDataStore;
            _oAuth2Clients = oAuth2Clients;
        }

        public async Task<string> GenerateAuthorizationUri
            (string clientId, string authorizationRedirectUri)
        {
            var oAuth2Client = _oAuth2Clients.FirstOrDefault
                (t => t.ClientCredentialOptions.ClientId == clientId);
            if (oAuth2Client == null)
            {
                throw new OAuth2MultiClientIntegratorFailureException
                    ("No suitable OAuthClient found.");
            }

            var authorizationState = $"{Guid.NewGuid()},{clientId}";
            await _oAuth2ClientDataStore.SetAuthorizationState
                (oAuth2Client.ClientCredentialOptions, authorizationState);

            var fullAuthorizationUri = oAuth2Client.AuthorizationCodeOptions
                .GenerateFullAuthorizationUri(clientId, authorizationState,
                    authorizationRedirectUri);
            return fullAuthorizationUri;
        }
    }
}
