using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2MultiClientIntegrator.Connectors.Extensions;
using OAuth2MultiClientIntegrator.Exceptions;

namespace OAuth2MultiClientIntegrator.Connectors.ActionFilters
{
    internal sealed class OAuth2AuthenticationRedirectSignalExceptionFilter : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled &&
                exceptionContext.Exception is InvalidOAuth2AccessTokenException)
            {
                var oauth2AuthenticationUriGenerator = exceptionContext.HttpContext
                    .RequestServices.GetRequiredService<IOAuth2AuthenticationUriGenerator>();

                var authenticationRedirectUri =
                    exceptionContext.HttpContext.Request.CreateAuthenticationRedirectUri();
                var clientId = exceptionContext.Exception.Message;
                var authenticationUri = await oauth2AuthenticationUriGenerator.
                    GenerateAuthenticationUri(clientId, authenticationRedirectUri);

                exceptionContext.ExceptionHandled = true;
                if (exceptionContext.HttpContext
                    .Request.IsAjaxRequest())
                {
                    exceptionContext.Result =
                        new OkObjectResult(new
                        {
                            isRedirected = true,
                            redirectUri = authenticationUri
                        });
                }
                else
                {
                    exceptionContext.HttpContext
                        .Response.Redirect(authenticationUri);
                }
            }
        }
    }
}
