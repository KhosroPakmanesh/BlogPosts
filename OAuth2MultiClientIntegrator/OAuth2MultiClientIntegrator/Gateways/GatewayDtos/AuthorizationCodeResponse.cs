using OAuth2MultiClientIntegrator.Models;

namespace OAuth2MultiClientIntegrator.Gateways.GatewayDtos
{
    internal sealed class AuthorizationCodeResponse
    {
        public const byte MarginTime = 10;
        public AuthorizationCodeResponse
            (string authorizationCode, int expiresIn, DateTime issuingDateTime)
        {
            AuthorizationCode = authorizationCode;
            ExpiresIn = expiresIn;
            IssuingDateTime = issuingDateTime;
        }

        public AuthorizationCodeStatus GetStatus(DateTime currentDateTime)
        {
            var expirationDateTime = IssuingDateTime
                .AddSeconds(ExpiresIn - MarginTime);

            if (expirationDateTime >= currentDateTime)
            {
                return AuthorizationCodeStatus.Valid;
            }

            return AuthorizationCodeStatus.Invalid;
        }

        public string AuthorizationCode { get; private set; }
        public int ExpiresIn { get; private set; }
        public DateTime IssuingDateTime { get; private set; }
    }
}
