using OAuth2MultiClientIntegrator.Models;

namespace OAuth2MultiClientIntegrator.Gateways.GatewayDtos
{
    internal sealed class AuthenticationCodeResponse
    {
        public const byte MarginTime = 10;
        public AuthenticationCodeResponse
            (string authenticationCode, int expiresIn, DateTime issuingDateTime)
        {
            AuthenticationCode = authenticationCode;
            ExpiresIn = expiresIn;
            IssuingDateTime = issuingDateTime;
        }

        public AuthenticationCodeStatus GetStatus(DateTime currentDateTime)
        {
            var expirationDateTime = IssuingDateTime
                .AddSeconds(ExpiresIn - MarginTime);

            if (expirationDateTime >= currentDateTime)
            {
                return AuthenticationCodeStatus.Valid;
            }

            return AuthenticationCodeStatus.Invalid;
        }

        public string AuthenticationCode { get; private set; }
        public int ExpiresIn { get; private set; }
        public DateTime IssuingDateTime { get; private set; }
    }
}
