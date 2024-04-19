namespace ArwynFr.Authentication.Proxy.Discord;

public record User
{
    public bool Bot { get; set; }

    public string DiscriminatedUsername => $"{Username}#{Discriminator}";

    public string Discriminator { get; set; } = string.Empty;

    public long Id { get; set; }

    public string Username { get; set; } = string.Empty;
}
