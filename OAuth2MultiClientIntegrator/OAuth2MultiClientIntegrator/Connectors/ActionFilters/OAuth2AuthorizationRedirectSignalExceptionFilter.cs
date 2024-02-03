using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2MultiClientIntegrator.Connectors.Extensions;
using OAuth2MultiClientIntegrator.Exceptions;

namespace OAuth2MultiClientIntegrator.Connectors.ActionFilters
{
    internal sealed class OAuth2AuthorizationRedirectSignalExceptionFilter : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled &&
                exceptionContext.Exception is InvalidOAuth2AccessTokenException)
            {
                var oauth2AuthorizationUriGenerator = exceptionContext.HttpContext
                    .RequestServices.GetRequiredService<IOAuth2AuthorizationUriGenerator>();

                var authorizationRedirectUri =
                    exceptionContext.HttpContext.Request.CreateAuthorizationRedirectUri();
                var clientId = exceptionContext.Exception.Message;
                var authorizationUri = await oauth2AuthorizationUriGenerator.
                    GenerateAuthorizationUri(clientId, authorizationRedirectUri);

                exceptionContext.ExceptionHandled = true;
                if (exceptionContext.HttpContext
                    .Request.IsAjaxRequest())
                {
                    exceptionContext.Result =
                        new OkObjectResult(new
                        {
                            isRedirected = true,
                            redirectUri = authorizationUri
                        });
                }
                else
                {
                    exceptionContext.HttpContext
                        .Response.Redirect(authorizationUri);
                }
            }
        }
    }
}
