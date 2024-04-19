using System.ComponentModel.DataAnnotations;

namespace ArwynFr.Authentication.Proxy.Connect;

public record ClientOptions
{
    public const string SectionName = "Clients";

    [Required]
    public string ClientId { get; init; } = string.Empty;

    public string[] Audiences { get; init; } = [];

    public Uri[] RedirectUris { get; init; } = [];

    public string[] Scopes { get; init; } = [];
}
