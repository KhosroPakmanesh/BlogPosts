namespace OAuth2MultiClientIntegrator
{
    internal interface IOAuth2TargetUriGenerator
    {
        Task<string> GenerateTargetUri
            (string authorizationCode, string authorizationState);
    }
}
