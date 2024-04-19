using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ArwynFr.Authentication.Proxy.Connect;

public class SeedDescriptorsFactory
{
    public OpenIddictApplicationDescriptor MakeDescriptor(ClientOptions client)
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = client.ClientId,
            DisplayName = client.ClientId,
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

        foreach (var uri in client.RedirectUris)
        {
            descriptor.RedirectUris.Add(uri);
        }

        foreach (var oauth_scope in client.Scopes)
        {
            descriptor.Permissions.Add(Permissions.Prefixes.Scope + oauth_scope);
        }

        return descriptor;
    }
}