using Newtonsoft.Json;
using OAuth2MultiClientIntegrator.Models;

namespace OAuth2MultiClientIntegrator.Gateways.GatewayDtos
{
    internal sealed class AccessTokenResponse
    {
        public const byte MarginTime = 10;

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime IssuingDateTime { get; set; }
            = DateTime.UtcNow;

        public AccessTokenStatus GetStatus
            (DateTime currentDateTime, int staticExpirationValue,
                bool canRenewAfterExpiration = true)
        {
            DateTime expirationDateTime;
            if (!canRenewAfterExpiration)
            {
                expirationDateTime = IssuingDateTime
                    .AddSeconds(staticExpirationValue - MarginTime);
                var tokenValidityPeriod = expirationDateTime - IssuingDateTime;
                var tokenValidityPeriodHalf = IssuingDateTime.Add(tokenValidityPeriod / 2);
                if (currentDateTime > tokenValidityPeriodHalf)
                {
                    return AccessTokenStatus.NearExpiration;
                }
            }
            else
            {
                expirationDateTime = IssuingDateTime
                    .AddSeconds(ExpiresIn - MarginTime);
            }

            if (currentDateTime < expirationDateTime)
            {
                return AccessTokenStatus.Valid;
            }

            return AccessTokenStatus.Invalid;
        }
    }
}
