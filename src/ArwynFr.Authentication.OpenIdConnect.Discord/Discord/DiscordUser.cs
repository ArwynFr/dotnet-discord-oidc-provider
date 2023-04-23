namespace ArwynFr.Authentication.OpenIdConnect.Discord.Discord;

public class DiscordUser
{
    public bool Bot { get; set; }

    public string DiscriminatedUsername => $"{Username}#{Discriminator}";

    public string Discriminator { get; set; } = string.Empty;

    public long Id { get; set; }

    public string Username { get; set; } = string.Empty;
}
