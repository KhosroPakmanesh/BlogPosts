using OAuth2MultiClientIntegrator.Gateways.GatewayDtos;

namespace OAuth2MultiClientIntegrator.Gateways
{
    internal interface IOAuth2ClientGateway
    {
        Task<AccessTokenResponse> GetAccessTokenResponse(string clientId, string authorizationCode);
        Task<AccessTokenResponse> GetRefreshAccessTokenResponse(string clientId, string refreshToken);
    }
}
