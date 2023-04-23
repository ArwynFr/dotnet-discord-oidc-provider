namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public class ClientOptions
{
    public string[] AdditionalScopes { get; set; } = new string[0];

    public string[] Audiences { get; set; } = new string[0];

    public Uri[] RedirectUris { get; set; } = new Uri[0];
}
