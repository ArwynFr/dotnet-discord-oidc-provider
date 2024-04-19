namespace ArwynFr.Authentication.Proxy.Connect;

public record ClientOptions
{
    public string[] Audiences { get; set; } = [];

    public Uri[] RedirectUris { get; set; } = [];

    public string[] Scopes { get; set; } = [];
}
