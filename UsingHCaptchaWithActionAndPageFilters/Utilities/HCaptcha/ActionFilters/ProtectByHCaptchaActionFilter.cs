using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha;

namespace UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ProtectByHCaptchaActionFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync
            (ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var hCaptchaVerifier = context.HttpContext
                .RequestServices.GetService<HCaptchaVerifier>();

            string hCaptchaToken =
                context.HttpContext.Request.Form["h-captcha-response"];

            var verificationResult =
                await hCaptchaVerifier.Verify(hCaptchaToken);
            if (!verificationResult)
            {
                context.Result =
                    new BadRequestObjectResult("hCaptcha Token is invalid!");
                return;
            }

            await next();
        }
    }
}