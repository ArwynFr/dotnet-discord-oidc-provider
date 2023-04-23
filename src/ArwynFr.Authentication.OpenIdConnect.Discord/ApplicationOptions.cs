namespace ArwynFr.Authentication.OpenIdConnect.Discord;

public class ApplicationOptions
{
    public const string SectionName = "Application";

    public Uri Contact { get; init; }

    public string Name { get; set; } = string.Empty;
}
