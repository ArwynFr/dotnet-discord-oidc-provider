using ArwynFr.Authentication.OpenIdConnect.Discord;
using ArwynFr.Authentication.OpenIdConnect.Discord.Secrets;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddSecretsSupport(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    // HTTPS and CORS are handled by infrastructure in production
    IdentityModelEventSource.ShowPII = true;
    app.UseHttpsRedirection();
    app.UseCors();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.UseStaticFiles();
app.UseDirectoryBrowser();

await app.RunAsync();
