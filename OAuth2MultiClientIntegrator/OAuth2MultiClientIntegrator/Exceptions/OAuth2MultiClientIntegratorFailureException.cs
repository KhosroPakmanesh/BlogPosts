namespace OAuth2MultiClientIntegrator.Exceptions
{
    internal sealed class OAuth2MultiClientIntegratorFailureException : Exception
    {
        public OAuth2MultiClientIntegratorFailureException()
        {

        }
        public OAuth2MultiClientIntegratorFailureException(string message) : base(message)
        {
        }
    }
}
