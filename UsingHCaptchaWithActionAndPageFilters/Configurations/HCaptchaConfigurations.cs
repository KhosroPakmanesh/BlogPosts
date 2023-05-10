using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha;

namespace UsingHCaptchaWithActionAndPageFilters.Configurations;

public static class HCaptchaConfigurations
{
    public static WebApplicationBuilder ConfigureHCaptcha
        (this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("hCaptcha", httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://hcaptcha.com/");
        });

        builder.Services.AddSingleton<HCaptchaVerifier>();
        builder.Services.AddSingleton<HCaptchaConfigurationProvider>();

        return builder;
    }
}
