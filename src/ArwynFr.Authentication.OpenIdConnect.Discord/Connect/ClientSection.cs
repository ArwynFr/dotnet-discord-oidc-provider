namespace ArwynFr.Authentication.OpenIdConnect.Discord.Connect;

public class ClientSection
{
    public Dictionary<string, ClientOptions> Clients { get; set; } = new Dictionary<string, ClientOptions>();
}
