using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha;

namespace UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha.PageFilters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigureHCaptchaPageFilter : Attribute, IAsyncPageFilter
    {
        public Task OnPageHandlerSelectionAsync
            (PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync
            (PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            await next();

            if (context.HandlerInstance is PageModel pageModel)
            {
                var hCaptchaConfigurationProvider = context.HttpContext
                    .RequestServices.GetService<HCaptchaConfigurationProvider>();

                pageModel.ViewData["hCaptchaSitekey"] =
                    hCaptchaConfigurationProvider.GetSiteKey();
            }
        }
    }
}