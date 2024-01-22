namespace OAuth2MultiClientIntegrator.Exceptions
{
    internal sealed class PossibleOAuth2CsrfAttackException : Exception
    {
        public PossibleOAuth2CsrfAttackException()
        {

        }
        public PossibleOAuth2CsrfAttackException(string message) : base(message)
        {
        }
    }
}
