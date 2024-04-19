using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public class AuthorizeController : ControllerBase
{
    public const string Route = "/connect/authorize";

    [Authorize, HttpPost(Route), HttpGet(Route)]
    public IActionResult Authorize()
    {
        var identity = ClaimsIdentityFactory.MakeClaimsIdentity(User);
        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
