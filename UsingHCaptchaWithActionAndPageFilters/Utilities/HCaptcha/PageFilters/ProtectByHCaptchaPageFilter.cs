using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha.PageFilters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtectByHCaptchaPageFilter : Attribute, IAsyncPageFilter
    {
        public Task OnPageHandlerSelectionAsync
            (PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync
            (PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HandlerMethod.MethodInfo.Name == "OnPostAsync")
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
            }

            await next();
        }
    }
}