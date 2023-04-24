using InvalidIdentityAccessHandler =  
    InvalidIdentityAccessHandlerMiddleware.Middlewares
    .InvalidIdentityAccessHandlerMiddleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InvalidIdentityAccessHandlerMiddleware.Test
{
    public class InvalidIdentityAccessHandlerMiddlewareTests
    {
        [Fact]
        public async Task InvalidIdentityAccessHandlerMiddlewareShould_Redirect_GivenNotAllowedRoutes()
        {
            //Arrange
            var middleware = new InvalidIdentityAccessHandler(
                nextRequestDelegate: (innerHttpContext) =>
                {
                    return Task.CompletedTask;
                }
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            context.Request.RouteValues = new RouteValueDictionary
            {
                 { "area", "identity"},
                 { "page", "/account/Register" }
            };

            //Act
            await middleware.InvokeAsync(context);
            
            //Assert
            Assert.Equal(StatusCodes.Status302Found, context.Response.StatusCode);
        }
        [Fact]
        public async Task InvalidIdentityAccessHandlerMiddlewareShould_Return200_GivenLoginRoute()
        {
            //Arrange
            var middleware = new InvalidIdentityAccessHandler(
                nextRequestDelegate: (innerHttpContext) =>
                {
                    return Task.CompletedTask;
                }
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            context.Request.RouteValues = new RouteValueDictionary
            {
                 { "area", "identity"},
                 { "page", "/account/login" }
            };

            //Act
            await middleware.InvokeAsync(context);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
        [Fact]
        public async Task InvalidIdentityAccessHandlerMiddlewareShould_Return200_GivenLogoutRoute()
        {
            //Arrange
            var middleware = new InvalidIdentityAccessHandler(
                nextRequestDelegate: (innerHttpContext) =>
                {
                    return Task.CompletedTask;
                }
            );
            
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            context.Request.RouteValues = new RouteValueDictionary
            {
                 { "area", "identity"},
                 { "page", "/account/logout" }
            };

            //Act
            await middleware.InvokeAsync(context);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }
}