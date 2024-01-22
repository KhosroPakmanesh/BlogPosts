namespace MVCWebApplication.AppConfigurations.OAuth2MultiClientIntegrator.Options
{
    public class OAuth2IntegrationsOptions
    {
        public const string OAuth2Integrations = "OAuth2Integrations";
        public ARegularClientSettings ARegularClientSettings { get; set; }
        public FacebookSettings FacebookSettings { get; set; }
        public GoogleSettings GoogleSettings { get; set; }
    }

    public class ARegularClientSettings
    {
        public ClientSettings ClientSettings { get; set; }
    }
    public class FacebookSettings
    {
        public ClientSettings ClientSettings { get; set; }
    }
    public class GoogleSettings
    {
        public ClientSettings ClientSettings { get; set; }
    }
    public class ClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TargetUri { get; set; }
    }
}
