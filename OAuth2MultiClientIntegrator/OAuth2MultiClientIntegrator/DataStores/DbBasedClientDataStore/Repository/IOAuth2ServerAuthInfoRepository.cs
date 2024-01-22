using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.DbDtos;

namespace OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.Repository
{
    internal interface IOAuth2ServerAuthInfoRepository
    {
        Task<OAuth2ServerAuthInfo> Get(string clientId);
        Task<OAuth2ServerAuthInfo> Add(OAuth2ServerAuthInfo oauth2ServerAuthInfo);
        Task Update(OAuth2ServerAuthInfo oauth2ServerAuthInfo);
        Task Delete(OAuth2ServerAuthInfo oauth2ServerAuthInfo);
        Task CommitAsync();
    }
}
