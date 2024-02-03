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
            (string authorizationCode, string authorizationState)
        {
            var dataNotReturnedCorrectly =
                authorizationCode == null ||
                authorizationState == null;
            if (dataNotReturnedCorrectly)
            {
                throw new OAuth2MultiClientIntegratorFailureException
                    ("IdentityServer callback data is missing.");
            }

            var clientId = authorizationState!.Split(',').LastOrDefault();
            var oAuth2Client = _oAuth2Clients.FirstOrDefault
                (t => t.ClientCredentialOptions.ClientId == clientId);
            if (oAuth2Client == null)
            {
                throw new OAuth2MultiClientIntegratorFailureException
                    ("No suitable OAuthClient found.");
            }

            var expectedAuthorizationState = await
                _oauth2ClientDataStore.GetAuthorizationState(clientId);
            if (expectedAuthorizationState != authorizationState)
            {
                throw new PossibleOAuth2CsrfAttackException();
            }

            var authorizationCodeResponse = new AuthorizationCodeResponse
                    (authorizationCode!, oAuth2Client.AuthorizationCodeOptions
                    .StaticExpirationValue, _dateTimeProvider.GetUTCDateTimeNow);

            await _oauth2ClientDataStore.SetAuthorizationCodeResponse
                    (oAuth2Client.ClientCredentialOptions, authorizationCodeResponse);

            return oAuth2Client.AuthorizationCodeOptions.TargetUri;
        }
    }
}
