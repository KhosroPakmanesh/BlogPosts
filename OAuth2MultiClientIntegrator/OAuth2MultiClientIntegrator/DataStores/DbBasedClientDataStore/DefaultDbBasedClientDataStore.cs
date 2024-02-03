using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.DbDtos;
using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.Repository;
using OAuth2MultiClientIntegrator.Gateways.GatewayDtos;

namespace OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore
{
    internal sealed class DefaultDbBasedClientDataStore : IOAuth2ClientDataStore
    {
        private readonly IOAuth2ServerAuthInfoRepository _oAuth2ServerAuthInfoRepository;

        public DefaultDbBasedClientDataStore(
            IOAuth2ServerAuthInfoRepository oauth2ServerAuthInfoRepository)
        {
            _oAuth2ServerAuthInfoRepository = oauth2ServerAuthInfoRepository;
        }

        public async Task<AuthorizationCodeResponse>
            GetAuthorizationCodeResponse(string clientId)
        {
            var oAuth2ServerAuthInfo = await
                _oAuth2ServerAuthInfoRepository.Get(clientId);
            if (oAuth2ServerAuthInfo != null)
            {
                return oAuth2ServerAuthInfo.AuthorizationCodeResponse;
            }

            return default;
        }
        public async Task SetAuthorizationCodeResponse(
            ClientCredentialOptions clientCredentialOptions,
            AuthorizationCodeResponse authorizationCodeResponse)
        {
            var oAuth2ServerAuthInfo = await _oAuth2ServerAuthInfoRepository
                .Get(clientCredentialOptions.ClientId);
            if (oAuth2ServerAuthInfo == null)
            {
                var newOAuth2ServerAuthInfo =
                    new OAuth2ServerAuthInfo(clientCredentialOptions,
                        authorizationCodeResponse);
                await _oAuth2ServerAuthInfoRepository
                    .Add(newOAuth2ServerAuthInfo);
            }
            else
            {
                oAuth2ServerAuthInfo.UpdateAuthorizationCodeResponse
                    (authorizationCodeResponse);
                await _oAuth2ServerAuthInfoRepository
                    .Update(oAuth2ServerAuthInfo);
            }

            await _oAuth2ServerAuthInfoRepository.CommitAsync();
        }

        public async Task<string> GetAuthorizationState(string clientId)
        {
            var oAuth2ServerAuthInfo = await
                _oAuth2ServerAuthInfoRepository.Get(clientId);
            if (oAuth2ServerAuthInfo != null)
            {
                return oAuth2ServerAuthInfo.AuthorizationState;
            }

            return default;
        }
        public async Task SetAuthorizationState(
            ClientCredentialOptions clientCredentialOptions
            , string authorizationState)
        {
            var oAuth2ServerAuthInfo =
                await _oAuth2ServerAuthInfoRepository
                .Get(clientCredentialOptions.ClientId);
            if (oAuth2ServerAuthInfo == null)
            {
                var newOAuth2ServerAuthInfo =
                    new OAuth2ServerAuthInfo(clientCredentialOptions,
                    authorizationState);
                await _oAuth2ServerAuthInfoRepository
                    .Add(newOAuth2ServerAuthInfo);
            }
            else
            {
                oAuth2ServerAuthInfo
                    .UpdateAuthorizationState(authorizationState);
                await _oAuth2ServerAuthInfoRepository
                    .Update(oAuth2ServerAuthInfo);
            }

            await _oAuth2ServerAuthInfoRepository.CommitAsync();
        }

        public async Task<AccessTokenResponse> GetAccessTokenResponse(string clientId)
        {
            var oAuth2ServerAuthInfo =
                await _oAuth2ServerAuthInfoRepository.Get(clientId);
            if (oAuth2ServerAuthInfo != null)
            {
                return oAuth2ServerAuthInfo.AccessTokenResponse;
            }

            return default;
        }
        public async Task SetAccessTokenResponse(
            ClientCredentialOptions clientCredentialOptions,
            AccessTokenResponse accessTokenResponse)
        {
            var oAuth2ServerAuthInfo =
                await _oAuth2ServerAuthInfoRepository
                .Get(clientCredentialOptions.ClientId);
            if (oAuth2ServerAuthInfo == null)
            {
                var newOAuth2ServerAuthInfo =
                    new OAuth2ServerAuthInfo(clientCredentialOptions,
                    accessTokenResponse);
                await _oAuth2ServerAuthInfoRepository
                    .Add(newOAuth2ServerAuthInfo);
            }
            else
            {
                oAuth2ServerAuthInfo
                    .UpdateAccessTokenResponse(accessTokenResponse);
                await _oAuth2ServerAuthInfoRepository
                    .Update(oAuth2ServerAuthInfo);
            }

            await _oAuth2ServerAuthInfoRepository.CommitAsync();
        }

        public async Task ClearDataStore(string clientId)
        {
            var oAuth2ServerAuthInfo =
                await _oAuth2ServerAuthInfoRepository.Get(clientId);
            if (oAuth2ServerAuthInfo != null)
            {
                await _oAuth2ServerAuthInfoRepository
                    .Delete(oAuth2ServerAuthInfo);
                await _oAuth2ServerAuthInfoRepository.CommitAsync();
            }
        }
    }
}
