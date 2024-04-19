using ArwynFr.Authentication.Proxy;
using ArwynFr.Authentication.Proxy.Connect;
using ArwynFr.Authentication.Proxy.Discord;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(ApplicationOptions.SectionName));
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddDiscordAuthentication(builder.Configuration.GetDiscordOptions());
builder.Services.AddOpenIdConnectServices();
builder.UseDevelopmentCors();

var app = builder.Build();

if (!app.Environment.IsProduction()) { app.UseCors(); }
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.UseStaticFiles();
app.UseDirectoryBrowser();
app.MapControllers();

await app.RunAsync();
