using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public class TokenController : ControllerBase
{
    public const string Route = "/connect/token";

    private const string Error = "Client id not found";

    private readonly ClientSection options;

    public TokenController(ClientSection options) => this.options = options;

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme), HttpPost(Route)]
    public IActionResult Token()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        if (request?.ClientId == null || !options.Clients.ContainsKey(request.ClientId))
        {
            return Unauthorized(new { message = Error });
        }

        var identity = ClaimsIdentityFactory.MakeClaimsIdentity(User);
        identity.SetScopes(request.GetScopes());
        identity.SetAudiences(options.Clients[request.ClientId].Audiences);
        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
