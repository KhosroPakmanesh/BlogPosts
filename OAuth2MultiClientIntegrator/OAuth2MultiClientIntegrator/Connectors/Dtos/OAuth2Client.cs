namespace OAuth2MultiClientIntegrator.Connectors.Dtos
{
    public sealed class OAuth2Client
    {
        public OAuth2Client(
            ClientCredentialOptions clientCredentialOptions,
            AuthorizationCodeOptions authorizationCodeOptions,
            AccessTokenOptions accessTokenOptions,
            RefreshTokenOptions refreshTokenOptions)
        {
            ClientCredentialOptions = clientCredentialOptions;
            AuthorizationCodeOptions = authorizationCodeOptions;
            AccessTokenOptions = accessTokenOptions;
            RefreshTokenOptions = refreshTokenOptions;

            SetBaseRefreshTokenUriFieldIfNecessary(accessTokenOptions);
        }
        public OAuth2Client(
            ClientCredentialOptions clientCredentialOptions,
            AuthorizationCodeOptions authorizationCodeOptions,
            AccessTokenOptions accessTokenOptions)
        {
            ClientCredentialOptions = clientCredentialOptions;
            AuthorizationCodeOptions = authorizationCodeOptions;
            AccessTokenOptions = accessTokenOptions;
            RefreshTokenOptions = new RefreshTokenOptions();

            SetBaseRefreshTokenUriFieldIfNecessary(accessTokenOptions);
        }


        public ClientCredentialOptions ClientCredentialOptions { get; set; }
        public AuthorizationCodeOptions AuthorizationCodeOptions { get; set; }
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
