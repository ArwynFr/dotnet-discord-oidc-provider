using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public class SeedDescriptorsFactory
{
    public OpenIddictApplicationDescriptor MakeDescriptor(string id, ClientOptions data)
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = id,
            DisplayName = id,
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

        foreach (var uri in data.RedirectUris)
        {
            descriptor.RedirectUris.Add(uri);
        }

        foreach (var oauth_scope in data.Scopes)
        {
            descriptor.Permissions.Add(Permissions.Prefixes.Scope + oauth_scope);
        }

        return descriptor;
    }
}