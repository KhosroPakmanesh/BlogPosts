namespace OAuth2MultiClientIntegrator.Connectors.Dtos
{
    public sealed class AuthorizationCodeOptions
    {
        public string BaseAuthorizationUri { get; set; }
        public string TargetUri { get; set; } = "/";
        public string Scope { get; set; }
        public string AccessType { get; set; } = "offline";
        public string ResponseType { get; set; } = "code";
        public int StaticExpirationValue { get; set; } = 600;
        public List<Tuple<string, string>> StandardAlternativeNamePairs { get; set; }
            = new List<Tuple<string, string>>();
        public List<KeyValuePair<string, string>> NonestandardSettings { get; set; }
            = new List<KeyValuePair<string, string>>();

        public string GenerateFullAuthorizationUri(
            string clientId, string authorizationState, string authorizationRedirectUri)
        {
            string fullAuthorizationUri = $"{BaseAuthorizationUri}?" +
                $"client_id={clientId}" +
                $"&redirect_uri={authorizationRedirectUri}" +
                $"&scope={Scope}" +
                $"&state={authorizationState}" +
                $"&access_type={AccessType}" +
                $"&response_type={ResponseType}";

            foreach (var NonestandardSetting in NonestandardSettings)
            {
                fullAuthorizationUri +=
                    $"&{NonestandardSetting.Key}={NonestandardSetting.Value}";
            }

            foreach (var alternativeNamesForStandardName in StandardAlternativeNamePairs)
            {
                var fullAuthorizationUriContainsAlternativeName =
                    fullAuthorizationUri.Contains(alternativeNamesForStandardName.Item1);
                if (fullAuthorizationUriContainsAlternativeName)
                {
                    fullAuthorizationUri.Replace
                        (alternativeNamesForStandardName.Item1,
                        alternativeNamesForStandardName.Item2);
                }
            }

            return fullAuthorizationUri;
        }
    }
}
