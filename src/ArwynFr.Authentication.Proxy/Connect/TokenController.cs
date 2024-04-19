using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace ArwynFr.Authentication.Proxy.Connect;

public class TokenController(IOptions<List<ClientOptions>> options) : ControllerBase
{
    public const string Route = "/connect/token";

    private const string Error = "Client id not found";

    private readonly IOptions<List<ClientOptions>> options = options;

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme), HttpPost(Route)]
    public IActionResult Token()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        var client = options.Value.FirstOrDefault(client => client.ClientId == request?.ClientId);
        if (request?.ClientId == null || client == null)
        {
            return Unauthorized(new { message = Error });
        }

        var identity = ClaimsIdentityFactory.MakeClaimsIdentity(User);
        identity.SetScopes(request.GetScopes());
        identity.SetAudiences(client.Audiences);
        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
