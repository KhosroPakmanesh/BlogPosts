namespace OAuth2MultiClientIntegrator.Connectors.Dtos
{
    public sealed class AuthenticationCodeOptions
    {
        public string BaseAuthenticationUri { get; set; }
        public string TargetUri { get; set; } = "/";
        public string Scope { get; set; }
        public string AccessType { get; set; } = "offline";
        public string ResponseType { get; set; } = "code";
        public int StaticExpirationValue { get; set; } = 600;
        public List<Tuple<string, string>> StandardAlternativeNamePairs { get; set; }
            = new List<Tuple<string, string>>();
        public List<KeyValuePair<string, string>> NonestandardSettings { get; set; }
            = new List<KeyValuePair<string, string>>();

        public string GenerateFullAuthenticationUri(
            string clientId, string authenticationState, string authenticationRedirectUri)
        {
            string fullAuthenticationUri = $"{BaseAuthenticationUri}?" +
                $"client_id={clientId}" +
                $"&redirect_uri={authenticationRedirectUri}" +
                $"&scope={Scope}" +
                $"&state={authenticationState}" +
                $"&access_type={AccessType}" +
                $"&response_type={ResponseType}";

            foreach (var NonestandardSetting in NonestandardSettings)
            {
                fullAuthenticationUri +=
                    $"&{NonestandardSetting.Key}={NonestandardSetting.Value}";
            }

            foreach (var alternativeNamesForStandardName in StandardAlternativeNamePairs)
            {
                var fullAuthenticationUriContainsAlternativeName =
                    fullAuthenticationUri.Contains(alternativeNamesForStandardName.Item1);
                if (fullAuthenticationUriContainsAlternativeName)
                {
                    fullAuthenticationUri.Replace
                        (alternativeNamesForStandardName.Item1,
                        alternativeNamesForStandardName.Item2);
                }
            }

            return fullAuthenticationUri;
        }
    }
}
