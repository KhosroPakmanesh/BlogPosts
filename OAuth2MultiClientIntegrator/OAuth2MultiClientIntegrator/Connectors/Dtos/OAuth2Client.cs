namespace OAuth2MultiClientIntegrator.Connectors.Dtos
{
    public sealed class OAuth2Client
    {
        public OAuth2Client(
            ClientCredentialOptions clientCredentialOptions,
            AuthenticationCodeOptions authenticationCodeOptions,
            AccessTokenOptions accessTokenOptions,
            RefreshTokenOptions refreshTokenOptions)
        {
            ClientCredentialOptions = clientCredentialOptions;
            AuthenticationCodeOptions = authenticationCodeOptions;
            AccessTokenOptions = accessTokenOptions;
            RefreshTokenOptions = refreshTokenOptions;

            SetBaseRefreshTokenUriFieldIfNecessary(accessTokenOptions);
        }
        public OAuth2Client(
            ClientCredentialOptions clientCredentialOptions,
            AuthenticationCodeOptions authenticationCodeOptions,
            AccessTokenOptions accessTokenOptions)
        {
            ClientCredentialOptions = clientCredentialOptions;
            AuthenticationCodeOptions = authenticationCodeOptions;
            AccessTokenOptions = accessTokenOptions;
            RefreshTokenOptions = new RefreshTokenOptions();

            SetBaseRefreshTokenUriFieldIfNecessary(accessTokenOptions);
        }


        public ClientCredentialOptions ClientCredentialOptions { get; set; }
        public AuthenticationCodeOptions AuthenticationCodeOptions { get; set; }
        public AccessTokenOptions AccessTokenOptions { get; set; }
        public RefreshTokenOptions RefreshTokenOptions { get; set; }

        private void SetBaseRefreshTokenUriFieldIfNecessary
            (AccessTokenOptions accessTokenOptions)
        {
            if (string.IsNullOrWhiteSpace
                (RefreshTokenOptions.BaseRefreshTokenUri))
            {
                RefreshTokenOptions.BaseRefreshTokenUri =
                    accessTokenOptions.BaseAccessTokenUri;
            }
        }
    }
}
