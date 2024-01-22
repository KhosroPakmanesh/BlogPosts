namespace OAuth2MultiClientIntegrator
{
    internal interface IOAuth2TargetUriGenerator
    {
        Task<string> GenerateTargetUri
            (string authenticationCode, string authenticationState);
    }
}
