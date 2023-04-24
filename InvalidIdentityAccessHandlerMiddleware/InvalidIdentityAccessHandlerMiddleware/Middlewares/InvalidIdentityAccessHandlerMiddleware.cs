using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace InvalidIdentityAccessHandlerMiddleware.Middlewares
{
    public class InvalidIdentityAccessHandlerMiddleware
    {
        private readonly RequestDelegate _nextRequestDelegate;
        private readonly string _targetArea = "identity";
        private readonly IList<string> _targetAllowedRoutes =
            new ReadOnlyCollection<string>
            (
                new List<string>
                {
                    "/account/login",
                    "/account/logout"
                }
            );

        public InvalidIdentityAccessHandlerMiddleware(RequestDelegate nextRequestDelegate)
        {
            _nextRequestDelegate = nextRequestDelegate;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var area = httpContext.GetRouteValue("area") as string;
            var page = httpContext.GetRouteValue("page") as string;

            if (area?.ToLower() == _targetArea &&
                !_targetAllowedRoutes.Contains(page?.ToLower()!))
            {
                httpContext.Response.Redirect("/home/index");
                return;
            }

            await _nextRequestDelegate(httpContext);
        }
    }
    public static class InvalidIdentityAccessHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseInvalidIdentityAccessHandler(
            this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<InvalidIdentityAccessHandlerMiddleware>();
        }
    }
}
