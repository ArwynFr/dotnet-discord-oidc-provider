using System.ComponentModel.DataAnnotations;

namespace ArwynFr.Authentication.OpenIdConnect.Discord;

public record ApplicationOptions
{
    public const string SectionName = "Application";

    [Required]
    public Uri Contact { get; init; }

    [Required]
    public string Name { get; init; }
}
