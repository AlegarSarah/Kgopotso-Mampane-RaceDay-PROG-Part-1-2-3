using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers;

public class AccountController : BaseController
{
    public AccountController(ApiClient api) : base(api) { }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginVm());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        HttpContext.Session.Clear();

        var res = await Api.Post<LoginResponse>(
            "api/auth/login",
            new
            {
                email = vm.Email,
                password = vm.Password
            });

        if (!res.Success || res.Data is null ||
            string.IsNullOrWhiteSpace(res.Data.Token))
        {
            ModelState.AddModelError("",
                res.Error.Length > 0
                    ? res.Error
                    : "Login failed. Please check your details.");

            return View(vm);
        }

        HttpContext.Session.SetString("JwtToken", res.Data.Token);
        HttpContext.Session.SetInt32("UserId", res.Data.User.UserId);
        HttpContext.Session.SetString("Role", res.Data.User.Role);
        HttpContext.Session.SetString("Name", res.Data.User.FirstName);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterVm());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var res = await Api.Post<object>(
            "api/auth/register",
            new
            {
                firstName = vm.FirstName,
                lastName = vm.LastName,
                email = vm.Email,
                password = vm.Password,
                role = vm.Role,
                phoneNumber = vm.PhoneNumber
            });

        if (!res.Success)
        {
            ModelState.AddModelError("", res.Error);
            return View(vm);
        }

        TempData["Success"] =
            "Account created successfully. You can now log in.";

        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        TempData["Success"] = "You have been logged out.";

        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}