using ArwynFr.Authentication.Proxy.Discord;

public static class ConfigurationExtensions
{
    public static DiscordOptions GetDiscordOptions(this IConfiguration configuration)
    => configuration.GetSection(DiscordOptions.ConfigurationPath).Get<DiscordOptions>() ?? new();
}