using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Filters;
using RaceDay.Web.Models;
using RaceDay.Web.Services;
namespace RaceDay.Web.Controllers;

[RoleAuthorize("Organiser")]
public class OrganiserController : BaseController
{
    public OrganiserController(ApiClient api) : base(api) { }

    private static object EventBody(EventFormVm vm) => new
    {
        name = vm.Name,
        description = vm.Description,
        date = vm.Date,
        location = vm.Location,
        distanceKm = vm.DistanceKm,
        eventType = vm.EventType
    };

    // ---------- Dashboard & events ----------
    public async Task<IActionResult> Dashboard()
    {
        var res = await Api.Get<List<EventSummaryVm>>("api/events/mine");
        if (res.StatusCode == 401) return ToLogin();
        Show(res);
        return View(res.Data ?? new List<EventSummaryVm>());
    }

    public async Task<IActionResult> Events()
    {
        var res = await Api.Get<List<EventSummaryVm>>("api/events/mine");
        if (res.StatusCode == 401) return ToLogin();
        Show(res);
        return View(res.Data ?? new List<EventSummaryVm>());
    }

    [HttpGet] public IActionResult CreateEvent() => View("EventForm", new EventFormVm());

    [HttpPost]
    public async Task<IActionResult> CreateEvent(EventFormVm vm)
    {
        if (!ModelState.IsValid) return View("EventForm", vm);

        var res = await Api.Post<EventVm>("api/events", EventBody(vm));
        if (res.StatusCode == 401) return ToLogin();
        if (!res.Success || res.Data is null)
        {
            ModelState.AddModelError("", res.Error);
            return View("EventForm", vm);
        }

        if (vm.Banner is { Length: > 0 })
        {
            var up = await Api.Upload<object>($"api/events/{res.Data.EventId}/banner", vm.Banner);
            if (!up.Success) TempData["Error"] = "Event saved, but the banner failed: " + up.Error;
        }
        if (TempData["Error"] is null) TempData["Success"] = "Event created.";
        return RedirectToAction(nameof(Events));
    }

    [HttpGet]
    public async Task<IActionResult> EditEvent(int id)
    {
        var res = await Api.Get<EventVm>($"api/events/{id}");
        if (res.StatusCode == 401) return ToLogin();
        if (!res.Success || res.Data is null)
        {
            TempData["Error"] = res.Error;
            return RedirectToAction(nameof(Events));
        }
        var e = res.Data;
        return View("EventForm", new EventFormVm
        {
            EventId = e.EventId,
            Name = e.Name,
            Description = e.Description,
            Date = e.Date,
            Location = e.Location,
            DistanceKm = e.DistanceKm,
            EventType = e.EventType,
            BannerUrl = e.BannerUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> EditEvent(EventFormVm vm)
    {
        if (!ModelState.IsValid) return View("EventForm", vm);

        var res = await Api.Put($"api/events/{vm.EventId}", EventBody(vm));
        if (res.StatusCode == 401) return ToLogin();
        if (!res.Success)
        {
            ModelState.AddModelError("", res.Error);
            return View("EventForm", vm);
        }

        if (vm.Banner is { Length: > 0 })
        {
            var up = await Api.Upload<object>($"api/events/{vm.EventId}/banner", vm.Banner);
            if (!up.Success) TempData["Error"] = "Event saved, but the banner failed: " + up.Error;
        }
        if (TempData["Error"] is null) TempData["Success"] = "Event updated.";
        return RedirectToAction(nameof(Events));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var res = await Api.Delete($"api/events/{id}");
        if (res.StatusCode == 401) return ToLogin();
        if (res.Success) TempData["Success"] = "Event deleted."; else TempData["Error"] = res.Error;
        return RedirectToAction(nameof(Events));
    }

    // ---------- Categories ----------
    public async Task<IActionResult> Categories(int eventId)
    {
        var ev = await Api.Get<EventVm>($"api/events/{eventId}");
        if (ev.StatusCode == 401) return ToLogin();
        if (!ev.Success || ev.Data is null)
        {
            TempData["Error"] = ev.Error;
            return RedirectToAction(nameof(Events));
        }

        var linked = (await Api.Get<List<CategoryVm>>($"api/events/{eventId}/categories")).Data ?? new();
        var all = (await Api.Get<List<CategoryVm>>("api/categories")).Data ?? new();
        return View(new CategoriesVm
        {
            Event = ev.Data,
            Linked = linked,
            Available = all.Where(c => linked.All(l => l.CategoryId != c.CategoryId)).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(int eventId, string? name, string categoryType)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            TempData["Error"] = "Please enter a category name.";
            return RedirectToAction(nameof(Categories), new { eventId });
        }

        var created = await Api.Post<CategoryVm>("api/categories", new { name = name.Trim(), categoryType });
        if (created.StatusCode == 401) return ToLogin();

        int? categoryId = created.Data?.CategoryId;
        if (created.StatusCode == 409)   // name already exists: reuse it
        {
            var all = await Api.Get<List<CategoryVm>>("api/categories");
            categoryId = all.Data?.FirstOrDefault(c => c.Name == name.Trim())?.CategoryId;
        }
        else if (!created.Success)
        {
            TempData["Error"] = created.Error;
            return RedirectToAction(nameof(Categories), new { eventId });
        }

        return await Link(eventId, categoryId ?? 0);
    }

    [HttpPost]
    public Task<IActionResult> LinkCategory(int eventId, int categoryId) => Link(eventId, categoryId);

    private async Task<IActionResult> Link(int eventId, int categoryId)
    {
        var res = await Api.Post<object>($"api/events/{eventId}/categories/{categoryId}");
        if (res.StatusCode == 401) return ToLogin();
        if (res.Success) TempData["Success"] = "Category added to the event."; else TempData["Error"] = res.Error;
        return RedirectToAction(nameof(Categories), new { eventId });
    }

    // ---------- Enrolments ----------
    public async Task<IActionResult> Enrolments(int eventId)
    {
        var ev = await Api.Get<EventVm>($"api/events/{eventId}");
        if (ev.StatusCode == 401) return ToLogin();
        if (!ev.Success || ev.Data is null)
        {
            TempData["Error"] = ev.Error;
            return RedirectToAction(nameof(Events));
        }

        var list = await Api.Get<List<EnrolmentVm>>($"api/events/{eventId}/enrolments");
        Show(list);
        return View(new EnrolmentsPageVm { Event = ev.Data, Enrolments = list.Data ?? new() });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int enrolmentId, int eventId, string status)
    {
        var res = await Api.Put($"api/enrolments/{enrolmentId}/status", new { status });
        if (res.StatusCode == 401) return ToLogin();
        if (res.Success) TempData["Success"] = $"Enrolment marked as {status}."; else TempData["Error"] = res.Error;
        return RedirectToAction(nameof(Enrolments), new { eventId });
    }

    // ---------- Results ----------
    public async Task<IActionResult> Results(int eventId)
    {
        var ev = await Api.Get<EventVm>($"api/events/{eventId}");
        if (ev.StatusCode == 401) return ToLogin();
        if (!ev.Success || ev.Data is null)
        {
            TempData["Error"] = ev.Error;
            return RedirectToAction(nameof(Events));
        }

        var enrolments = (await Api.Get<List<EnrolmentVm>>($"api/events/{eventId}/enrolments")).Data ?? new();
        var results = (await Api.Get<List<EventResultVm>>($"api/results/event/{eventId}")).Data ?? new();

        return View(new ResultsPageVm
        {
            Event = ev.Data,
            Rows = enrolments.Where(e => e.Status != "Cancelled")
                .Select(e => new ResultRowVm
                {
                    Enrolment = e,
                    Result = results.FirstOrDefault(r => r.EnrolmentId == e.EnrolmentId)
                }).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> SaveResult(int eventId, int enrolmentId, TimeSpan finishTime, int position)
    {
        var res = await Api.Post<object>("api/results", new { enrolmentId, finishTime, position, publish = true });
        if (res.StatusCode == 401) return ToLogin();
        if (res.Success) TempData["Success"] = "Result captured and published."; else TempData["Error"] = res.Error;
        return RedirectToAction(nameof(Results), new { eventId });
    }
}