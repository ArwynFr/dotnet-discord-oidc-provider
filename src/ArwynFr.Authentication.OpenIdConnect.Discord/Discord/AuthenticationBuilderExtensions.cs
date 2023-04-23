using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using OpenIddict.Abstractions;
using System.Net.Http.Headers;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Discord;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddDiscord(this AuthenticationBuilder builder, string authenticationScheme, string signInScheme, IConfigurationSection configuration) => builder
        .AddOAuth(authenticationScheme, DiscordDefaults.DisplayName, options =>
        {
            var userOptions = configuration.Get<DiscordOptions>() ?? new DiscordOptions();
            options.ClientId = userOptions.ClientId;
            options.ClientSecret = userOptions.ClientSecret;
            options.CallbackPath = userOptions.CallbackPath;

            options.SignInScheme = signInScheme;
            options.AuthorizationEndpoint = DiscordDefaults.AuthorizationEndpoint;
            options.TokenEndpoint = DiscordDefaults.TokenEndpoint;
            options.Scope.Add(DiscordDefaults.IdentifyScope);
            options.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    try
                    {
                        context.Backchannel.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        var user = await context.Backchannel.GetFromJsonAsync<DiscordUser>(DiscordDefaults.UserInformationEndpoint).ConfigureAwait(false) ??
                            throw new InvalidOperationException();
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
            };
        });
}
