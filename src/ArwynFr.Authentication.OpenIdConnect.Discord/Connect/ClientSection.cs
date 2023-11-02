namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public class ClientSection
{
    public Dictionary<string, ClientOptions> Clients { get; set; } = new Dictionary<string, ClientOptions>();

    public record ClientOptions
    {
        public string[] Audiences { get; set; } = new string[0];

        public Uri[] RedirectUris { get; set; } = new Uri[0];

        public string[] Scopes { get; set; } = new string[0];
    }
}
