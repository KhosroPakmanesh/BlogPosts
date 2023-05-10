using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha
{
    public class HCaptchaConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public HCaptchaConfigurationProvider
            (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSecret()
        {
            return _configuration
                .GetValue<string>("HCaptcha:Secret");
        }

        public string GetSiteKey()
        {
            return _configuration
                .GetValue<string>("HCaptcha:SiteKey");
        }

        public bool IsDevelopment
        {
            get
            {
                return _configuration
                    .GetValue<bool>("HCaptcha:DevelopmentMode");
            }
        }
    }
}
