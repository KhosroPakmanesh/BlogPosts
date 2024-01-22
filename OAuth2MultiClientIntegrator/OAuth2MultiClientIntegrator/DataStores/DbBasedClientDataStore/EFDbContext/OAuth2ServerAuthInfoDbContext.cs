using Microsoft.EntityFrameworkCore;
using OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.DbDtos;

namespace OAuth2MultiClientIntegrator.DataStores.DbBasedClientDataStore.EFDbContext
{
    internal partial class OAuth2ServerAuthInfoDbContext : DbContext
    {
        public OAuth2ServerAuthInfoDbContext(DbContextOptions<OAuth2ServerAuthInfoDbContext> options) : base(options) { }

        public virtual DbSet<OAuth2ServerAuthInfo> OAuth2ServerAuthInfos { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //The following command helps EntityFramework find configuration files in the current assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(
                System.Reflection.Assembly.GetExecutingAssembly());

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<OAuth2ServerAuthInfo>(entity =>
            {
                entity.ToTable("oauth2_server_auth_infos", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ClientName).HasColumnName("client_name");
                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.OwnsOne(x => x.AuthenticationCodeResponse,
                    authenticationCodeResponse =>
                    {
                        authenticationCodeResponse.Property
                            (x => x.AuthenticationCode).HasColumnName("acr_code");
                        authenticationCodeResponse.Property
                            (x => x.ExpiresIn).HasColumnName("acr_expires_in");
                        authenticationCodeResponse.Property
                            (x => x.IssuingDateTime)
                            .HasColumnType("smalldatetime")
                            .HasColumnName("acr_issuing_date_time");
                    });

                entity.Property(e => e.AuthenticationState).HasColumnName("authentication_state");

                entity.OwnsOne(x => x.AccessTokenResponse,
                    accessTokenResponse =>
                    {
                        accessTokenResponse.Property
                            (x => x.AccessToken).HasColumnName("atr_access_token");
                        accessTokenResponse.Property
                            (x => x.ExpiresIn).HasColumnName("atr_expires_in");
                        accessTokenResponse.Property
                            (x => x.IssuingDateTime)
                            .HasColumnType("smalldatetime")
                            .HasColumnName("atr_issuing_date_time");
                        accessTokenResponse.Property
                            (x => x.RefreshToken).HasColumnName("atr_refresh_token");
                    });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
