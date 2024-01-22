namespace OAuth2MultiClientIntegrator
{
    public interface IOAuth2AccessTokenProvider
    {
        Task<string> GetAccessToken(string clientName);
    }
}
