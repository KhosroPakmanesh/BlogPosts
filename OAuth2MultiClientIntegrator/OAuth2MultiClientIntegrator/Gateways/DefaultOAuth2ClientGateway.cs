using Newtonsoft.Json;
using OAuth2MultiClientIntegrator.Connectors.Dtos;
using OAuth2MultiClientIntegrator.Connectors.Extensions;
using OAuth2MultiClientIntegrator.Exceptions;
using OAuth2MultiClientIntegrator.Gateways.GatewayDtos;
using System.Net.Http.Headers;
using System.Text;

namespace OAuth2MultiClientIntegrator.Gateways
{
    internal sealed class DefaultOAuth2ClientGateway : IOAuth2ClientGateway
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<OAuth2Client> _oAuth2Clients;

        public DefaultOAuth2ClientGateway(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            List<OAuth2Client> oAuth2Clients)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _oAuth2Clients = oAuth2Clients;
        }

        public async Task<AccessTokenResponse> GetAccessTokenResponse
            (string clientId, string authorizationCode)
        {
            var oauth2Client = GetOAuth2Client(clientId);

            // Create the HttpClient
            HttpClient httpClient = _httpClientFactory.CreateClient();

            // Set the Basic Authentication header
            var authValue = new AuthenticationHeaderValue
                ("Basic", Convert.ToBase64String
                    (Encoding.UTF8.GetBytes(
                    $"{oauth2Client.ClientCredentialOptions.ClientId}:" +
                    $"{oauth2Client.ClientCredentialOptions.ClientSecret}")));
            httpClient.DefaultRequestHeaders.Authorization = authValue;

            // Prepare the request body
            var authenticationRedirectUri = _httpContextAccessor
                .HttpContext.Request.CreateAuthenticationRedirectUri();
            var requestBody = new FormUrlEncodedContent(
                oauth2Client.AccessTokenOptions.GenerateSettings
                (authorizationCode, authenticationRedirectUri));

            AccessTokenResponse accessTokenResponse = null;

            try
            {
                // Send the POST request
                var response = await httpClient.PostAsync(
                    oauth2Client.AccessTokenOptions.BaseAccessTokenUri, requestBody);

                // Read the response content
                var responseContent =
                    await response.Content.ReadAsStringAsync();

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    accessTokenResponse = JsonConvert.
                        DeserializeObject<AccessTokenResponse>(responseContent);

                    SetRefreshTokenFieldIfNecessary(accessTokenResponse);
                    SetExpiresInFieldIfNecessary(oauth2Client, accessTokenResponse);
                }
            }
            catch (Exception exception)
            {
                throw new OAuth2MultiClientIntegratorFailureException(exception.Message);
            }

            return accessTokenResponse;
        }
        public async Task<AccessTokenResponse> GetRefreshAccessTokenResponse
            (string clientId, string refreshToken)
        {
            var oauth2Client = GetOAuth2Client(clientId);

            // Create the HttpClient
            HttpClient httpClient = _httpClientFactory.CreateClient();

            // Set the Basic Authentication header
            var authValue = new AuthenticationHeaderValue
                ("Basic", Convert.ToBase64String
                    (Encoding.UTF8.GetBytes(
                    $"{oauth2Client.ClientCredentialOptions.ClientId}:" +
                    $"{oauth2Client.ClientCredentialOptions.ClientSecret}")));
            httpClient.DefaultRequestHeaders.Authorization = authValue;

            // Prepare the request body
            var requestBody = new FormUrlEncodedContent(
                oauth2Client.RefreshTokenOptions.GenerateSettings(refreshToken));

            AccessTokenResponse accessTokenResponse = null;

            try
            {
                // Send the POST request
                var response = await httpClient.PostAsync(
                    oauth2Client.RefreshTokenOptions.BaseRefreshTokenUri, requestBody);

                // Read the response content
                var responseContent =
                    await response.Content.ReadAsStringAsync();

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    accessTokenResponse = JsonConvert.
                        DeserializeObject<AccessTokenResponse>(responseContent);

                    SetRefreshTokenFieldIfNecessary(accessTokenResponse);
                    SetExpiresInFieldIfNecessary(oauth2Client, accessTokenResponse);
                }
            }
            catch (Exception exception)
            {
                throw new OAuth2MultiClientIntegratorFailureException(exception.Message);
            }

            return accessTokenResponse;
        }

        private OAuth2Client GetOAuth2Client(string clientId)
        {
            var oAuth2Client = _oAuth2Clients.FirstOrDefault
                (t => t.ClientCredentialOptions.ClientId == clientId);
            if (oAuth2Client == null)
            {
                throw new OAuth2MultiClientIntegratorFailureException
                    ("No suitable OAuthClient found.");
            }

            return oAuth2Client;
        }
        private static void SetExpiresInFieldIfNecessary
            (OAuth2Client oauth2Client,
            AccessTokenResponse accessTokenResponse)
        {
            if (!oauth2Client.AccessTokenOptions.CanRenewAfterExpiration &&
                accessTokenResponse.ExpiresIn == 0)
            {
                accessTokenResponse.ExpiresIn =
                    oauth2Client.AccessTokenOptions.StaticExpirationValue;
            }
        }
        private static void SetRefreshTokenFieldIfNecessary
            (AccessTokenResponse accessTokenResponse)
        {
            if (string.IsNullOrWhiteSpace(accessTokenResponse.RefreshToken))
            {
                accessTokenResponse.RefreshToken =
                    accessTokenResponse.AccessToken;
            }
        }
    }
}
