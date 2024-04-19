namespace ArwynFr.Authentication.Proxy.Discord;

public static class Defaults
{
    public const string AuthenticationScheme = "discord";

    public const string AuthorizationEndpoint = "https://discord.com/oauth2/authorize";

    public const string DefaultCallbackPath = "/signin-discord";

    public const string DisplayName = "Discord";

    public const string EmailScope = "email";

    public const string IdentifyScope = "identify";

    public const string TokenEndpoint = "https://discord.com/api/oauth2/token";

    public const string UserInformationEndpoint = "https://discord.com/api/users/@me";
}
