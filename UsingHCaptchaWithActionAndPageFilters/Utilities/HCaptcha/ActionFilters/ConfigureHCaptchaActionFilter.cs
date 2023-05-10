using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConfigureHCaptchaActionFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync
            (ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var hCaptchaConfigurationProvider = context.HttpContext
                .RequestServices.GetService<HCaptchaConfigurationProvider>();

            var controller = context.Controller as Controller;

            controller.ViewData["hCaptchaSitekey"] =
                hCaptchaConfigurationProvider.GetSiteKey();

            await next();
        }
    }
}