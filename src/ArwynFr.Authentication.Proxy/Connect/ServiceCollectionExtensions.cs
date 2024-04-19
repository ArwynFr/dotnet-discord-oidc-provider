using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace ArwynFr.Authentication.Proxy.Connect;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenIdConnectServices(this IServiceCollection services) => services
        .AddClientsOptions()
        .AddOpenIddictServices()
        .AddScoped<SeedDescriptorsCommand>()
        .AddScoped<SeedDescriptorsFactory>()
        .AddHostedService<SeedDescriptorsService>();

    private static IServiceCollection AddClientsOptions(this IServiceCollection services) => services
        .AddOptions<ClientSection>().BindConfiguration(ClientSection.SectionName).Services;

    private static IServiceCollection AddOpenIddictServices(this IServiceCollection services) => services
        .AddDbContext<DbContext>(_ => _.UseInMemoryDatabase(nameof(DbContext)).UseOpenIddict())
        .AddOpenIddict().AddCore(_ => _.UseEntityFrameworkCore().UseDbContext<DbContext>())
        .AddServer(ConfigureOpenIddictServer).Services;

    private static void ConfigureOpenIddictServer(OpenIddictServerBuilder options) => options
        .AddDevelopmentEncryptionCertificate()
        .AddDevelopmentSigningCertificate()
        .DisableAccessTokenEncryption()
        .AllowAuthorizationCodeFlow()
        .AllowRefreshTokenFlow()
        .RequireProofKeyForCodeExchange()
        .SetAuthorizationEndpointUris(AuthorizeController.Route)
        .SetTokenEndpointUris(TokenController.Route)
        .RegisterScopes(OpenIddictConstants.Scopes.Profile)
        .RegisterClaims(OpenIddictConstants.Claims.Name)
        .RegisterClaims(ClaimTypes.Name)
        .SetAccessTokenLifetime(TimeSpan.FromMinutes(20))
        .SetIdentityTokenLifetime(TimeSpan.FromMinutes(20))
        .SetRefreshTokenLifetime(TimeSpan.FromDays(7))
        .UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .EnableAuthorizationEndpointPassthrough();
}
