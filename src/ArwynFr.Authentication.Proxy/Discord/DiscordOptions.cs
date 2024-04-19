using System.ComponentModel.DataAnnotations;

namespace ArwynFr.Authentication.Proxy.Discord;

public record DiscordOptions
{
    public const string ConfigurationPath = "Discord";

    [Required]
    public string ClientId { get; init; } = string.Empty;

    [Required]
    public string ClientSecret { get; init; } = string.Empty;
}
