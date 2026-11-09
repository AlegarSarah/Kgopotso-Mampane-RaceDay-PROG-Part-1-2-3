using Microsoft.AspNetCore.Mvc;
namespace RaceDay.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return HttpContext.Session.GetString("Role") switch
        {
            "Organiser" => RedirectToAction("Dashboard", "Organiser"),
            "Participant" => RedirectToAction("Index", "Participant"),
            _ => View()
        };
    }

    public IActionResult Error() => View();
}