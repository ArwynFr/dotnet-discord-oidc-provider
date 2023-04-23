using ArwynFr.Authentication.OpenIdConnect.Discord.Connect;
using ArwynFr.Authentication.OpenIdConnect.Discord.Discord;

namespace ArwynFr.Authentication.OpenIdConnect.Discord;

internal static class ServiceCollectionExtensions
{
    private const string CookieAuthenticationScheme = "cookie";

    private const string DiscordAuthenticationScheme = "discord";

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) => services
        .Configure<ApplicationOptions>(configuration.GetSection(ApplicationOptions.SectionName))
        .AddRazorPages().Services
        .AddHttpContextAccessor()
        .AddControllers().Services
        .AddApplicationCorsPolicy()
        .AddApplicationAuthentication(configuration)
        .AddOpenIddictServerServices();

    private static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration) => services
        .AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
        })
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationScheme;
            options.DefaultChallengeScheme = DiscordAuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationScheme)
        .AddDiscord(DiscordAuthenticationScheme, CookieAuthenticationScheme, configuration.GetSection(DiscordOptions.ConfigurationPath))
        .Services;

    private static IServiceCollection AddApplicationCorsPolicy(this IServiceCollection services) => services
        .AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(
                        "https://localhost:5002",
                        "https://localhost:5003"
                    );
            });
        });
}
