namespace LogEnabledHttpClient.Extensions
{
    public static class HttpClientFactoryExtensions
    {
        public static HttpClient CreateLogEnabledClient
            (this IHttpClientFactory serviceCollection)
        {
            return serviceCollection.CreateClient("LogEnabledClient");
        }
    }
}
