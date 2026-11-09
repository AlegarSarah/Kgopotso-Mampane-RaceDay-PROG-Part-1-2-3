using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace RaceDay.Web.Filters;

/// Protects pages: not logged in -> Login, wrong role -> Access denied.
public class RoleAuthorizeAttribute : ActionFilterAttribute
{
    private readonly string? _role;
    public RoleAuthorizeAttribute(string? role = null) => _role = role;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        if (session.GetInt32("UserId") is null)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }
        if (_role is not null && session.GetString("Role") != _role)
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
    }
}