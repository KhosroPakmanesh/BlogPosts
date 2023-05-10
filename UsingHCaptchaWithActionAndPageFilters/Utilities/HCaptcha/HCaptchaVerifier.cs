using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha
{
    public class HCaptchaVerifier
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HCaptchaConfigurationProvider _hCaptchaConfigurationProvider;

        public HCaptchaVerifier(IHttpClientFactory clientFactory,
            HCaptchaConfigurationProvider hCaptchaConfigurationProvider)
        {
            _clientFactory = clientFactory;
            _hCaptchaConfigurationProvider = hCaptchaConfigurationProvider;
        }

        public async Task<bool> Verify(string responseToken)
        {
            if (_hCaptchaConfigurationProvider.IsDevelopment)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(responseToken))
            {
                return false;
            }

            var hCaptchaResult = await MakeApiCallToHCaptchaServer(
                    _hCaptchaConfigurationProvider.GetSecret(), responseToken);

            return hCaptchaResult.Success;
        }
        private async Task<HCaptchaResult> MakeApiCallToHCaptchaServer
            (string secret, string responseToken)
        {
            var client = _clientFactory.CreateClient("hCaptcha");

            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("secret", secret),
                new KeyValuePair<string, string>("response", responseToken),
            };

            var httpResponseMessage = await client.PostAsync(
                "/siteverify", new FormUrlEncodedContent(postData));

            return await httpResponseMessage
                .Content.ReadFromJsonAsync<HCaptchaResult>();
        }
    }
}
