using ArwynFr.Authentication.Proxy.Connect;

public static class WebApplicationBuilderExtensions
{
    public static void UseDevelopmentCors(this WebApplicationBuilder app)
    {
        if (app.Environment.IsProduction()) { return; }
        var options = app.Configuration.GetClientOptions();
        var origins = options
            .SelectMany(client => client.RedirectUris)
            .Select(uri => $"{uri.Scheme}://{uri.Authority}")
            .Distinct().ToArray();

        app.Services.AddCors(_ => _.AddDefaultPolicy(_ => _.WithOrigins(origins)));
    }

    private static List<ClientOptions> GetClientOptions(this IConfiguration configuration)
    => configuration.GetSection(ClientOptions.SectionName).Get<List<ClientOptions>>() ?? [];
}