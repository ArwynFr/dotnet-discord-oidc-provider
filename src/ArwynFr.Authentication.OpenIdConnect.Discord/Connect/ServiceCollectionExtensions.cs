using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenIddictServerServices(this IServiceCollection services) => services
        .AddDbContext<DbContext>(options =>
        {
            options.UseInMemoryDatabase(nameof(DbContext)).UseOpenIddict();
        })
        .AddOpenIddict()
        .AddCore(options =>
        {
            options.UseEntityFrameworkCore().UseDbContext<DbContext>();
        })
        .AddServer(options =>
        {
            options.AddDevelopmentEncryptionCertificate();
            options.AddDevelopmentSigningCertificate();
            options.DisableAccessTokenEncryption();
            options.AllowAuthorizationCodeFlow();
            options.RequireProofKeyForCodeExchange();
            options.SetAuthorizationEndpointUris("/connect/authorize");
            options.SetTokenEndpointUris("/connect/token");
            options.UseAspNetCore()
                .EnableTokenEndpointPassthrough()
                .EnableAuthorizationEndpointPassthrough();

            options.RegisterScopes(OpenIddictConstants.Scopes.Profile);
            options.RegisterClaims(OpenIddictConstants.Claims.Name);
            options.RegisterClaims(ClaimTypes.Name);
            options.SetAccessTokenLifetime(TimeSpan.FromMinutes(20));
            options.SetIdentityTokenLifetime(TimeSpan.FromMinutes(20));
        })
        .Services
        .AddClientsConfigurationProvider()
        .AddHostedService<SeedApplicationDescriptors>();

    private static IServiceCollection AddClientsConfigurationProvider(this IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("clients.json").Build();
        var section = config.Get<ClientSection>() ??
            throw new InvalidOperationException("Cannot load clients configuration file");
        return services.AddSingleton(section);
    }
}
