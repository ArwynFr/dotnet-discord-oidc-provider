namespace ArwynFr.Authentication.Proxy.Connect;

public class ClientSection
{
    public const string SectionName = "Clients";
    public Dictionary<string, ClientOptions> Clients { get; set; } = [];
}
