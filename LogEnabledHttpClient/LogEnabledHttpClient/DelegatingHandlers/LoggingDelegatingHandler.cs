using Microsoft.Extensions.Logging;

namespace LogEnabledHttpClient.DelegatingHandlers
{
    internal sealed class LoggingDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingDelegatingHandler> _logger;

        public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync
            (HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            var request = httpRequestMessage.ToString();
            var requestContent = string.Empty;
            if (httpRequestMessage.Content != null)
            {
                requestContent =
                    await httpRequestMessage.Content.ReadAsStringAsync();
            }

            var httpResponseMessage = await base.SendAsync
                (httpRequestMessage, cancellationToken);

            var response = httpResponseMessage.ToString();
            var responseContent = string.Empty;
            if (httpResponseMessage.Content != null)
            {
                responseContent =
                    await httpResponseMessage.Content.ReadAsStringAsync();
            }

            var logBody =
                $"\nHttp Request Information:\n" +
                $"{request}\n\n" +
                $"Http Request Content Information:\n" +
                $"{requestContent}\n\n" +
                $"Http Response Information:\n" +
                $"{response}\n\n" +
                $"Http Response Content Information:\n" +
                $"{responseContent}\n\n";

            _logger.LogInformation(logBody);

            return httpResponseMessage;
        }
    }
}
