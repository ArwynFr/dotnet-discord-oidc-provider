using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;

namespace ArwynFr.Authentication.Proxy.Pages;

[Authorize]
public class IndexModel : PageModel
{
    public void OnGet() => Expression.Empty();
}
