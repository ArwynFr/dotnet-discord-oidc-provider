using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace ArwynFr.Authentication.Proxy.Connect;

public class SeedDescriptorsCommand(
    IOptions<List<ClientOptions>> options,
    IOpenIddictApplicationManager manager,
    SeedDescriptorsFactory factory)
{
    private readonly IOptions<List<ClientOptions>> options = options;
    private readonly IOpenIddictApplicationManager manager = manager;
    private readonly SeedDescriptorsFactory factory = factory;

    public async Task ExecuteAsync(CancellationToken cancellation)
    {
        foreach (var client in options.Value)
        {
            var descriptor = factory.MakeDescriptor(client);
            await manager
                .CreateAsync(descriptor, cancellation)
                .ConfigureAwait(false);
        }
    }
}
