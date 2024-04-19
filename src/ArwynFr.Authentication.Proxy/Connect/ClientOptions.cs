using System.ComponentModel.DataAnnotations;

namespace ArwynFr.Authentication.Proxy.Connect;

public record ClientOptions
{
    public const string SectionName = "Clients";

    [Required]
    public required string ClientId { get; init; }

    public string[] Audiences { get; init; } = [];

    public Uri[] RedirectUris { get; init; } = [];

    public string[] Scopes { get; init; } = [];
}
