using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public class SeedDescriptorsCommand(
    IOptions<ClientSection> options,
    IOpenIddictApplicationManager manager,
    SeedDescriptorsFactory factory)
{
    private readonly IOptions<ClientSection> options = options;
    private readonly IOpenIddictApplicationManager manager = manager;
    private readonly SeedDescriptorsFactory factory = factory;

    public async Task ExecuteAsync(CancellationToken cancellation)
    {
        foreach (var (id, data) in options.Value.Clients)
        {
            var descriptor = factory.MakeDescriptor(id, data);
            await manager
                .CreateAsync(descriptor, cancellation)
                .ConfigureAwait(false);
        }
    }
}
