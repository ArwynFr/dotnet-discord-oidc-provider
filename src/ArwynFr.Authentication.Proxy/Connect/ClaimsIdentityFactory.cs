using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace ArwynFr.Authentication.Proxy.Connect;

public static class ClaimsIdentityFactory
{
    public static ClaimsIdentity MakeClaimsIdentity(ClaimsPrincipal principal)
    {
        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        identity.SetClaim(OpenIddictConstants.Claims.Subject, principal.FindFirstValue(OpenIddictConstants.Claims.Subject));
        identity.SetClaim(OpenIddictConstants.Claims.Name, principal.Identity?.Name);
        identity.SetClaim(ClaimTypes.Name, principal.Identity?.Name);
        identity.SetDestinations(claim => [
            OpenIddictConstants.Destinations.AccessToken,
            OpenIddictConstants.Destinations.IdentityToken]);
        return identity;
    }
}
