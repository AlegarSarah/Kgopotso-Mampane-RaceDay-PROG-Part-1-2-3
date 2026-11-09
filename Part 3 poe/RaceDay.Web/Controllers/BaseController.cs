using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
namespace RaceDay.Web.Controllers;

public abstract class BaseController : Controller
{
    protected readonly ApiClient Api;
    protected BaseController(ApiClient api) => Api = api;

    protected IActionResult ToLogin()
    {
        HttpContext.Session.Clear();
        TempData["Error"] = "Your session has expired. Please log in again.";
        return RedirectToAction("Login", "Account");
    }

    protected void Show(ApiResult r) { if (!r.Success) TempData["Error"] = r.Error; }
}