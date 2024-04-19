using Microsoft.AspNetCore.Authentication.OAuth;
using OpenIddict.Abstractions;
using System.Net.Http.Headers;

namespace ArwynFr.Authentication.Proxy.Discord;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDiscordAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var discordOptions = configuration.GetRequiredSection(DiscordOptions.ConfigurationPath)
            .Get<DiscordOptions>() ?? throw new InvalidOperationException();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Constants.CookieAuthenticationScheme;
                options.DefaultChallengeScheme = Constants.DiscordAuthenticationScheme;
            })
            .AddCookie(Constants.CookieAuthenticationScheme)
            .AddOAuth(Constants.DiscordAuthenticationScheme, Defaults.DisplayName, options =>
            {
                options.ClientId = discordOptions.ClientId;
                options.ClientSecret = discordOptions.ClientSecret;
                options.CallbackPath = discordOptions.CallbackPath;
                options.SignInScheme = Constants.CookieAuthenticationScheme;
                options.AuthorizationEndpoint = Defaults.AuthorizationEndpoint;
                options.TokenEndpoint = Defaults.TokenEndpoint;
                options.Scope.Add(Defaults.IdentifyScope);
                options.Events = new OAuthEvents { OnCreatingTicket = OnCreatingTicket };
            });

        return services;
    }

    private static async Task OnCreatingTicket(OAuthCreatingTicketContext context)
    {
        try
        {
            context.Backchannel.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            var user = await context.Backchannel
                .GetFromJsonAsync<User>(Defaults.UserInformationEndpoint)
                .ConfigureAwait(false) ?? throw new InvalidOperationException();
            _ = context.Identity ?? throw new InvalidOperationException();
            context.Identity.SetClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
            context.Identity.SetClaim(context.Identity.NameClaimType, user.DiscriminatedUsername);
            context.Success();
        }
        catch (Exception)
        {
            context.Fail("Cannot fetch user information from Discord ; check https://discordstatus.com/");
        }
    }
}
