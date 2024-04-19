namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public record ClientOptions
{
    public string[] Audiences { get; set; } = [];

    public Uri[] RedirectUris { get; set; } = [];

    public string[] Scopes { get; set; } = [];
}
