using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.DataStores;
using OAuth2MultiClientIntegrator.DateTimerProvider;
using OAuth2MultiClientIntegrator.Exceptions;
using OAuth2MultiClientIntegrator.Gateways.GatewayDtos;


namespace OAuth2MultiClientIntegrator
{
    internal sealed class DefaultOAuth2TargetUriGenerator : IOAuth2TargetUriGenerator
    {
        private readonly IOAuth2ClientDataStore _oauth2ClientDataStore;
        private readonly List<OAuth2Client> _oAuth2Clients;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DefaultOAuth2TargetUriGenerator(
            IOAuth2ClientDataStore oauth2ClientDataStore,
            List<OAuth2Client> oAuth2Clients,
            IDateTimeProvider dateTimeProvider)
        {
            _oauth2ClientDataStore = oauth2ClientDataStore;
            _oAuth2Clients = oAuth2Clients;
            _dateTimeProvider = dateTimeProvider;
        }
        public async Task<string> GenerateTargetUri
            (string authenticationCode, string authenticationState)
        {
            var dataNotReturnedCorrectly =
                authenticationCode == null ||
                authenticationState == null;
            if (dataNotReturnedCorrectly)
            {
                throw new OAuth2MultiClientIntegratorFailureException
                    ("IdentityServer callback data is missing.");
            }

            var clientId = authenticationState!.Split(',').LastOrDefault();
            var oAuth2Client = _oAuth2Clients.FirstOrDefault
                (t => t.ClientCredentialOptions.ClientId == clientId);
            if (oAuth2Client == null)
            {
                throw new OAuth2MultiClientIntegratorFailureException
                    ("No suitable OAuthClient found.");
            }

            var expectedAuthenticationState = await
                _oauth2ClientDataStore.GetAuthenticationState(clientId);
            if (expectedAuthenticationState != authenticationState)
            {
                throw new PossibleOAuth2CsrfAttackException();
            }

            var authenticationCodeResponse = new AuthenticationCodeResponse
                    (authenticationCode!, oAuth2Client.AuthenticationCodeOptions
                    .StaticExpirationValue, _dateTimeProvider.GetUTCDateTimeNow);

            await _oauth2ClientDataStore.SetAuthenticationCodeResponse
                    (oAuth2Client.ClientCredentialOptions, authenticationCodeResponse);

            return oAuth2Client.AuthenticationCodeOptions.TargetUri;
        }
    }
}
