namespace OAuth2MultiClientIntegrator.Exceptions
{
    internal sealed class InvalidOAuth2AccessTokenException : Exception
    {
        public InvalidOAuth2AccessTokenException()
        {

        }
        public InvalidOAuth2AccessTokenException(string message) : base(message)
        {
        }
    }
}
