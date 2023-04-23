using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

[ApiController, Route("/[controller]")]
public class ConnectController : ControllerBase
{
    private readonly ClientSection options;

    public ConnectController(ClientSection options)
    {
        this.options = options;
    }

    [Authorize, HttpPost("authorize"), HttpGet("authorize")]
    public IActionResult Authorize()
    {
        var identity = MakeClaimsIdentity();

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme), HttpPost("token")]
    public IActionResult Token()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.ClientId == null || !options.Clients.ContainsKey(request.ClientId))
        {
            return Unauthorized(new { message = "Client id not found" });
        }

        var identity = MakeClaimsIdentity();
        identity.SetScopes(request.GetScopes());
        identity.SetAudiences(options.Clients[request.ClientId].Audiences);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private ClaimsIdentity MakeClaimsIdentity()
    {
        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        identity.SetClaim(OpenIddictConstants.Claims.Subject, User.FindFirstValue(OpenIddictConstants.Claims.Subject));
        identity.SetClaim(OpenIddictConstants.Claims.Name, User.Identity?.Name);
        identity.SetClaim(ClaimTypes.Name, User.Identity?.Name);
        identity.SetDestinations(claim => new[] {
            OpenIddictConstants.Destinations.AccessToken,
            OpenIddictConstants.Destinations.IdentityToken});
        return identity;
    }
}
