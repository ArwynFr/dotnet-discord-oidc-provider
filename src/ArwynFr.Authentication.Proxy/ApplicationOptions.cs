using System.ComponentModel.DataAnnotations;

namespace ArwynFr.Authentication.Proxy;

public record ApplicationOptions
{
    public const string SectionName = "Application";

    [Required]
    public required Uri Contact { get; init; }

    [Required]
    public required string Name { get; init; }
}
