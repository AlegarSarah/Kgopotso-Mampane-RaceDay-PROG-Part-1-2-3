using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Filters;
using RaceDay.Web.Models;
using RaceDay.Web.Services;
namespace RaceDay.Web.Controllers;

[RoleAuthorize("Participant")]
public class ParticipantController : BaseController
{
    public ParticipantController(ApiClient api) : base(api) { }

    public async Task<IActionResult> Index(string? search, string? type, decimal? maxDistance)
    {
        var qs = new List<string> { "upcomingOnly=true" };
        if (!string.IsNullOrWhiteSpace(search)) qs.Add("search=" + Uri.EscapeDataString(search));
        if (!string.IsNullOrWhiteSpace(type)) qs.Add("type=" + Uri.EscapeDataString(type));
        if (maxDistance is not null) qs.Add("maxDistance=" + maxDistance.Value.ToString(CultureInfo.InvariantCulture));

        var res = await Api.Get<List<EventVm>>("api/events?" + string.Join("&", qs));
        if (res.StatusCode == 401) return ToLogin();
        Show(res);

        ViewBag.Search = search; ViewBag.Type = type; ViewBag.MaxDistance = maxDistance;
        return View(res.Data ?? new List<EventVm>());
    }

    public async Task<IActionResult> Details(int id)
    {
        var ev = await Api.Get<EventVm>($"api/events/{id}");
        if (ev.StatusCode == 401) return ToLogin();
        if (!ev.Success || ev.Data is null)
        {
            TempData["Error"] = ev.Error;
            return RedirectToAction(nameof(Index));
        }

        var cats = await Api.Get<List<CategoryVm>>($"api/events/{id}/categories");
        var mine = await Api.Get<List<EnrolmentVm>>("api/enrolments/mine");
        return View(new EventDetailsVm
        {
            Event = ev.Data,
            Categories = cats.Data ?? new(),
            AlreadyEnrolled = mine.Data?.Any(e => e.EventId == id) ?? false
        });
    }

    [HttpPost]
    public async Task<IActionResult> Enrol(int eventId, int categoryId)
    {
        if (categoryId == 0)
        {
            TempData["Error"] = "Please choose a category.";
            return RedirectToAction(nameof(Details), new { id = eventId });
        }

        var res = await Api.Post<object>("api/enrolments", new { eventId, categoryId });
        if (res.StatusCode == 401) return ToLogin();
        if (!res.Success)
        {
            TempData["Error"] = res.Error;
            return RedirectToAction(nameof(Details), new { id = eventId });
        }

        TempData["Success"] = "You are in! Your enrolment is pending confirmation.";
        return RedirectToAction(nameof(MyEvents));
    }

    public async Task<IActionResult> MyEvents()
    {
        var res = await Api.Get<List<EnrolmentVm>>("api/enrolments/mine");
        if (res.StatusCode == 401) return ToLogin();
        Show(res);
        return View(res.Data ?? new List<EnrolmentVm>());
    }

    public async Task<IActionResult> MyResults()
    {
        var res = await Api.Get<List<MyResultVm>>("api/results/mine");
        if (res.StatusCode == 401) return ToLogin();
        Show(res);
        return View(res.Data ?? new List<MyResultVm>());
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var res = await Api.Get<ProfileVm>("api/profile");
        if (res.StatusCode == 401) return ToLogin();
        Show(res);
        return View(res.Data ?? new ProfileVm());
    }

    [HttpPost]
    public async Task<IActionResult> Profile(ProfileVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var res = await Api.Put("api/profile", new
        {
            firstName = vm.FirstName,
            lastName = vm.LastName,
            phoneNumber = vm.PhoneNumber
        });
        if (res.StatusCode == 401) return ToLogin();
        if (!res.Success)
        {
            ModelState.AddModelError("", res.Error);
            return View(vm);
        }

        HttpContext.Session.SetString("Name", vm.FirstName);
        TempData["Success"] = "Profile updated.";
        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    public async Task<IActionResult> UploadPicture(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            TempData["Error"] = "Please choose an image first.";
            return RedirectToAction(nameof(Profile));
        }

        var res = await Api.Upload<object>("api/profile/picture", file);
        if (res.StatusCode == 401) return ToLogin();
        if (res.Success) TempData["Success"] = "Profile picture updated."; else TempData["Error"] = res.Error;
        return RedirectToAction(nameof(Profile));
    }
}