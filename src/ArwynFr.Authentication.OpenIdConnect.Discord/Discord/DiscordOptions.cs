namespace ArwynFr.Authentication.OpenIdConnect.Discord.Discord;

public class DiscordOptions
{
    public const string ConfigurationPath = "Discord";

    public PathString CallbackPath { get; set; } = new PathString(DiscordDefaults.DefaultCallbackPath);

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;
}
