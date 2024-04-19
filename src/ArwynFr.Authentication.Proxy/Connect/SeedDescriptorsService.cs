using Microsoft.EntityFrameworkCore;

namespace ArwynFr.Authentication.Proxy.Connect;

public class SeedDescriptorsService(IServiceScopeFactory scopeFactory) : IHostedService
{
    private readonly IServiceScopeFactory scopeFactory = scopeFactory;
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DbContext>();
        await context.Database.EnsureDeletedAsync().ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

        var command = scope.ServiceProvider.GetRequiredService<SeedDescriptorsCommand>();
        await command.ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
