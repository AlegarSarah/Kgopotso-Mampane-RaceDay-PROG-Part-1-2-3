using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;

namespace RaceDay.API.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public class RoutesController : ControllerBase
{
private readonly RaceDayDbContext _context;

public RoutesController(RaceDayDbContext context)
{
    _context = context;
}

// GET: api/Routes
[HttpGet]
public async Task<IActionResult> GetRoutes()
{
    var routes = await _context.Routes
        .AsNoTracking()
        .ToListAsync();

    return Ok(routes);
}

// GET: api/Routes/5
[HttpGet("{id:int}")]
public async Task<IActionResult> GetRoute(int id)
{
    var route = await _context.Routes.FindAsync(id);

    if (route == null)
    {
        return NotFound(new
        {
            message = "Route not found."
        });
    }

    return Ok(route);
}

// POST: api/Routes
[HttpPost]
public async Task<IActionResult> CreateRoute(
    [FromBody] RaceDay.API.Models.Route route)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    bool eventExists = await _context.Events
        .AnyAsync(e => e.EventId == route.EventId);

    if (!eventExists)
    {
        return BadRequest(new
        {
            message = "The specified event does not exist."
        });
    }

    route.RouteId = 0;

    _context.Routes.Add(route);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetRoute),
        new { id = route.RouteId },
        route);
}

// PUT: api/Routes/5
[HttpPut("{id:int}")]
public async Task<IActionResult> UpdateRoute(
    int id,
    [FromBody] RaceDay.API.Models.Route updatedRoute)
{
    if (id != updatedRoute.RouteId)
    {
        return BadRequest(new
        {
            message = "The route ID in the URL must match the route ID in the body."
        });
    }

    var existingRoute = await _context.Routes.FindAsync(id);

    if (existingRoute == null)
    {
        return NotFound(new
        {
            message = "Route not found."
        });
    }

    bool eventExists = await _context.Events
        .AnyAsync(e => e.EventId == updatedRoute.EventId);

    if (!eventExists)
    {
        return BadRequest(new
        {
            message = "The specified event does not exist."
        });
    }

    existingRoute.EventId = updatedRoute.EventId;
    existingRoute.DistanceKM = updatedRoute.DistanceKM;
    existingRoute.RouteDescription = updatedRoute.RouteDescription;
    existingRoute.RouteMapURL = updatedRoute.RouteMapURL;

    await _context.SaveChangesAsync();

    return NoContent();
}

// DELETE: api/Routes/5
[HttpDelete("{id:int}")]
public async Task<IActionResult> DeleteRoute(int id)
{
    var route = await _context.Routes.FindAsync(id);

    if (route == null)
    {
        return NotFound(new
        {
            message = "Route not found."
        });
    }

    _context.Routes.Remove(route);

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
        return Conflict(new
        {
            message = "The route could not be deleted because it is referenced by another database record."
        });
    }

    return NoContent();
}

}