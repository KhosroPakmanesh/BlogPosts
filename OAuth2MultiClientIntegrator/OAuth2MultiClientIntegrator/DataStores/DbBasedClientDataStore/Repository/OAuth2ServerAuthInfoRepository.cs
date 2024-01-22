using Microsoft.EntityFrameworkCore;
using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.DbDtos;
using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.EFDbContext;

namespace OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.Repository
{
    internal sealed class OAuth2ServerAuthInfoRepository : IOAuth2ServerAuthInfoRepository
    {
        private readonly OAuth2ServerAuthInfoDbContext _oauth2ServerAuthInfoDbContext;
        private bool _disposed;

        public OAuth2ServerAuthInfoRepository
            (OAuth2ServerAuthInfoDbContext oauth2ServerAuthInfoDbContext)
        {
            _oauth2ServerAuthInfoDbContext = oauth2ServerAuthInfoDbContext;
        }

        public async Task<OAuth2ServerAuthInfo> Get(string clientId)
        {
            return await _oauth2ServerAuthInfoDbContext
                .OAuth2ServerAuthInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    t => t.ClientId == clientId);
        }
        public async Task<OAuth2ServerAuthInfo> Add(OAuth2ServerAuthInfo oauth2ServerAuthInfo)
        {
            var entityEntry = await _oauth2ServerAuthInfoDbContext.
                OAuth2ServerAuthInfos.AddAsync(oauth2ServerAuthInfo);
            return entityEntry.Entity;
        }
        public Task Update(OAuth2ServerAuthInfo oauth2ServerAuthInfo)
        {
            _oauth2ServerAuthInfoDbContext
                .OAuth2ServerAuthInfos.Update(oauth2ServerAuthInfo);

            return Task.CompletedTask;
        }
        public Task Delete(OAuth2ServerAuthInfo oauth2ServerAuthInfo)
        {
            _oauth2ServerAuthInfoDbContext
                .Remove(oauth2ServerAuthInfo);

            return Task.CompletedTask;
        }
        public async Task CommitAsync()
        {
            await _oauth2ServerAuthInfoDbContext
                .SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
        {
            await Dispose(true);
            GC.SuppressFinalize(this);
        }
        private async Task Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await _oauth2ServerAuthInfoDbContext.DisposeAsync();
                }
            }
            _disposed = true;
        }
    }
}
