using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using System.Linq.Expressions;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Pages
{
    public class PrivacyModel : PageModel
    {
        public PrivacyModel(
            IOptions<ApplicationOptions> applicationOptions,
            IOptions<SessionOptions> sessionOptions,
            IOptions<OpenIddictServerOptions> openIddictOptions)
        {
            Contact = applicationOptions.Value.Contact;
            IdleTimeout = sessionOptions.Value.IdleTimeout;
            TokenLifetime = openIddictOptions.Value.AccessTokenLifetime ?? TimeSpan.FromHours(1);
            CodesLifetime = openIddictOptions.Value.AuthorizationCodeLifetime ?? TimeSpan.FromMinutes(5);
        }

        public TimeSpan CodesLifetime { get; set; }

        public Uri Contact { get; init; }

        public TimeSpan IdleTimeout { get; set; }

        public TimeSpan TokenLifetime { get; set; }

        public void OnGet() => Expression.Empty();
    }
}
