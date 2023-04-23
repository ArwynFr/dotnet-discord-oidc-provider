using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Secrets;

public static class ConfigurationBuilderExtensions
{
    public const string OverrideSecretsPath = "Application:SecretsPath";

    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "This is a default well-known value defined by docker here: https://docs.docker.com/engine/swarm/secrets/#how-docker-manages-secrets")]
    public const string PosixDefaultPath = "/run/secrets";

    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "This is a default well-known value defined by docker here: https://docs.docker.com/engine/swarm/secrets/#how-docker-manages-secrets")]
    public const string WindowsDefaultPath = "C:\\ProgramData\\Docker\\secrets";

    private static string DefaultPath => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? WindowsDefaultPath : PosixDefaultPath;

    public static IConfigurationBuilder AddSecretsSupport(this IConfigurationBuilder builder, IConfiguration configuration)
    {
        var path = configuration.GetValue<string>(OverrideSecretsPath) ?? DefaultPath;
        builder.AddKeyPerFile(path, optional: true);
        return builder;
    }
}
