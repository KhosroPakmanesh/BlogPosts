using OAuth2MultiClientIntegrator.Models;

namespace OAuth2MultiClientIntegrator.Connectors.Extensions
{
    internal static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }

        public static string CreateAuthorizationRedirectUri(this HttpRequest request)
        {
            return
                $"{request.Scheme}://" +
                $"{request.Host}/" +
                 OAuth2ClientManagerSettings.DefaultOAuth2RedirectUri;
        }
    }
}
