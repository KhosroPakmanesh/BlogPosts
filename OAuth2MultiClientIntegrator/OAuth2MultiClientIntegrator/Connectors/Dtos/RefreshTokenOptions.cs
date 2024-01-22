namespace OAuth2MultiClientIntegrator.Connectors.Dtos
{
    public sealed class RefreshTokenOptions
    {
        public string BaseRefreshTokenUri { get; set; }
        public string GrantType { get; set; } = "refresh_token";
        public int StaticExpirationValue { get; set; } = 0;
        public bool CanRenewAfterExpiration { get; set; } = true;
        public List<Tuple<string, string>> StandardAlternativeNamePairs { get; set; }
            = new List<Tuple<string, string>>();
        public List<KeyValuePair<string, string>> NonestandardSettings { get; set; }
            = new List<KeyValuePair<string, string>>();

        public List<KeyValuePair<string, string>> GenerateSettings(string refreshToken)
        {
            var settings = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            };
            settings.AddRange(NonestandardSettings);

            foreach (var AlternativeNamesForStandardName in
                StandardAlternativeNamePairs)
            {
                var oldSetting = settings
                    .FirstOrDefault(t => t.Key ==
                    AlternativeNamesForStandardName.Item1);

                settings.Add(new KeyValuePair<string, string>
                    (AlternativeNamesForStandardName.Item2, oldSetting.Value));

                settings.Remove(oldSetting);
            }

            return settings;
        }
    }
}
