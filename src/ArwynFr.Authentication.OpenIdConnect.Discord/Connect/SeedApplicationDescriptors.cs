using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public class SeedApplicationDescriptors : IHostedService
{
    private readonly IServiceScopeFactory scopeFactory;

    public SeedApplicationDescriptors(IServiceScopeFactory scopeFactory)
    {
        ArgumentNullException.ThrowIfNull(scopeFactory);
        this.scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DbContext>();
        await context.Database.EnsureDeletedAsync().ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var options = scope.ServiceProvider.GetRequiredService<ClientSection>();

        foreach (var client in options.Clients)
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = client.Key,
                DisplayName = client.Key,
                Type = ClientTypes.Public,
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.Authorization,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Profile,
                },
            };

            foreach (var uri in client.Value.RedirectUris)
            {
                descriptor.RedirectUris.Add(uri);
            }

            foreach (var oauth_scope in client.Value.Scopes)
            {
                descriptor.Permissions.Add(Permissions.Prefixes.Scope + oauth_scope);
            }

            await manager.CreateAsync(descriptor).ConfigureAwait(false);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
