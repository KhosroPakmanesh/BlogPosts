using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.DataStores;
using OAuth2MultiClientIntegrator.DateTimerProvider;
using OAuth2MultiClientIntegrator.Exceptions;
using OAuth2MultiClientIntegrator.Gateways;
using OAuth2MultiClientIntegrator.Gateways.GatewayDtos;
using OAuth2MultiClientIntegrator.Models;

namespace OAuth2MultiClientIntegrator
{
    internal sealed class DefaultOAuth2AccessTokenProvider : IOAuth2AccessTokenProvider
    {
        private readonly IOAuth2ClientGateway _oauth2ClientGateway;
        private readonly IOAuth2ClientDataStore _oAuth2ClientDataStore;
        private readonly List<OAuth2Client> _oAuth2Clients;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DefaultOAuth2AccessTokenProvider(
            IOAuth2ClientGateway iOAuth2ClientGateway,
            IOAuth2ClientDataStore oAuth2ClientDataStore,
            List<OAuth2Client> oAuth2Clients,
            IDateTimeProvider dateTimeProvider)
        {
            _oauth2ClientGateway = iOAuth2ClientGateway;
            _oAuth2ClientDataStore = oAuth2ClientDataStore;
            _oAuth2Clients = oAuth2Clients;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<string> GetAccessToken(string clientName)
        {
            var oAuth2Client = GetOAuth2ClientFromRegisteredClients(clientName);
            var clientId = oAuth2Client.ClientCredentialOptions.ClientId;

            var accessTokenResponse = await
                GetAccessTokenResponseFromDataStore(clientId);
            if (accessTokenResponse == null)
            {
                return await GetOrGenerateAccessToken(oAuth2Client);
            }

            return await GetAccessTokenOrGenerateRefreshAccessToken
                (oAuth2Client, accessTokenResponse);
        }

        private async Task<string> GetOrGenerateAccessToken(OAuth2Client oAuth2Client)
        {
            var clientId = oAuth2Client.ClientCredentialOptions.ClientId;

            var authorizationCodeResponse =
                await GetAuthorizationCodeResponseFromDataStore(clientId);
            if (authorizationCodeResponse != null)
            {
                var authorizationCodeStatus = authorizationCodeResponse
                    .GetStatus(_dateTimeProvider.GetUTCDateTimeNow);
                if (authorizationCodeStatus == AuthorizationCodeStatus.Valid)
                {
                    var accessTokenResponse =
                        await GetAccessTokenResponseFromGateway
                        (clientId, authorizationCodeResponse.AuthorizationCode);

                    if (accessTokenResponse == null)
                    {
                        throw await ClearDataStoreAndRaiseException(clientId);
                    }

                    await SetAccessTokenResponseToDataStore
                        (oAuth2Client.ClientCredentialOptions, accessTokenResponse);

                    return accessTokenResponse.AccessToken;
                }
            }

            throw await ClearDataStoreAndRaiseException(clientId);
        }
        private async Task<string> GetAccessTokenOrGenerateRefreshAccessToken
            (OAuth2Client oAuth2Client, AccessTokenResponse accessTokenResponse)
        {
            var clientId = oAuth2Client.ClientCredentialOptions.ClientId;

            var accessTokenStatus = accessTokenResponse
                .GetStatus(_dateTimeProvider.GetUTCDateTimeNow,
                oAuth2Client.RefreshTokenOptions.StaticExpirationValue,
                oAuth2Client.RefreshTokenOptions.CanRenewAfterExpiration);
            if (accessTokenStatus == AccessTokenStatus.Valid)
            {
                return accessTokenResponse.AccessToken;
            }
            else if (accessTokenStatus == AccessTokenStatus.Invalid ||
                accessTokenStatus == AccessTokenStatus.NearExpiration)
            {
                var newAccessTokenResponse =
                    await GetRefreshAccessTokenResponseFromGatway
                    (clientId, accessTokenResponse.RefreshToken);

                if (newAccessTokenResponse == null)
                {
                    throw await ClearDataStoreAndRaiseException(clientId);
                }

                await SetAccessTokenResponseToDataStore
                    (oAuth2Client.ClientCredentialOptions, newAccessTokenResponse);

                return newAccessTokenResponse.AccessToken;
            }

            throw await ClearDataStoreAndRaiseException(clientId);
        }

        private OAuth2Client GetOAuth2ClientFromRegisteredClients
            (string clientName)
        {
            var oAuth2Client = _oAuth2Clients.FirstOrDefault
                (t => t.ClientCredentialOptions.ClientName == clientName);
            if (oAuth2Client == null)
            {
                throw new OAuth2MultiClientIntegratorFailureException
                    ("No suitable OAuthClient found.");
            }

            return oAuth2Client;
        }
        private async Task<InvalidOAuth2AccessTokenException>
            ClearDataStoreAndRaiseException(string clientId)
        {
            await _oAuth2ClientDataStore.ClearDataStore(clientId);
            return new InvalidOAuth2AccessTokenException(clientId);
        }

        private async Task<AccessTokenResponse>
            GetAccessTokenResponseFromDataStore(string clientId)
        {
            return await _oAuth2ClientDataStore
                .GetAccessTokenResponse(clientId);
        }
        private async Task<AuthorizationCodeResponse>
            GetAuthorizationCodeResponseFromDataStore(string clientId)
        {
            return await _oAuth2ClientDataStore.
                GetAuthorizationCodeResponse(clientId);
        }
        private async Task SetAccessTokenResponseToDataStore
            (ClientCredentialOptions clientCredentialOptions,
            AccessTokenResponse accessTokenResponse)
        {
            await _oAuth2ClientDataStore.SetAccessTokenResponse
                (clientCredentialOptions, accessTokenResponse);
        }

        private async Task<AccessTokenResponse>
            GetAccessTokenResponseFromGateway
            (string clientId, string authorizationCode)
        {
            return await _oauth2ClientGateway.GetAccessTokenResponse
                (clientId, authorizationCode);
        }
        private async Task<AccessTokenResponse>
            GetRefreshAccessTokenResponseFromGatway
            (string clientId, string refreshToken)
        {
            return await _oauth2ClientGateway
                .GetRefreshAccessTokenResponse
                (clientId, refreshToken);
        }
    }
}