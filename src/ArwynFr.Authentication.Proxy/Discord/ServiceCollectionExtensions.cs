namespace ArwynFr.Authentication.Proxy.Discord;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDiscordAuthentication(this IServiceCollection services, DiscordOptions options) => services
        .AddAuthentication(_ =>
        {
            _.DefaultAuthenticateScheme = Constants.CookieAuthenticationScheme;
            _.DefaultChallengeScheme = Constants.DiscordAuthenticationScheme;
        })
        .AddCookie(Constants.CookieAuthenticationScheme)
        .AddDiscord(Constants.DiscordAuthenticationScheme, _ =>
        {
            _.ClientId = options.ClientId;
            _.ClientSecret = options.ClientSecret;
            _.SignInScheme = Constants.CookieAuthenticationScheme;
        }).Services;
}
