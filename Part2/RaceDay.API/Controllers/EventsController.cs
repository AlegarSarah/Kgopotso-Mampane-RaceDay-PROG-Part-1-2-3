using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventsController(RaceDayDbContext context)
    {
        _context = context;
    }

    // GET: api/Events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        var events = await _context.Events
            .AsNoTracking()
            .ToListAsync();

        return Ok(events);
    }

    // GET: api/Events/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var raceEvent = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EventId == id);

        if (raceEvent == null)
        {
            return NotFound(new
            {
                message = $"Event with ID {id} was not found."
            });
        }

        return Ok(raceEvent);
    }

    // POST: api/Events
    [Authorize(Roles = "Organiser")]
    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(Event raceEvent)
    {
        var organiserExists = await _context.Users
            .AnyAsync(u => u.UserId == raceEvent.OrganiserId);

        if (!organiserExists)
        {
            return BadRequest(new
            {
                message = "The specified organiser does not exist."
            });
        }

        raceEvent.EventId = 0;

        _context.Events.Add(raceEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEvent),
            new { id = raceEvent.EventId },
            raceEvent);
    }

    // PUT: api/Events/1
    [Authorize(Roles = "Organiser")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEvent(
        int id, Event updatedEvent)
    {
        if (id != updatedEvent.EventId)
        {
            return BadRequest(new
            {
                message = "The URL ID must match the event ID in the request."
            });
        }

        var raceEvent = await _context.Events.FindAsync(id);

        if (raceEvent == null)
        {
            return NotFound(new
            {
                message = $"Event with ID {id} was not found."
            });
        }

        var organiserExists = await _context.Users
            .AnyAsync(u => u.UserId == updatedEvent.OrganiserId);

        if (!organiserExists)
        {
            return BadRequest(new
            {
                message = "The specified organiser does not exist."
            });
        }

        raceEvent.OrganiserId = updatedEvent.OrganiserId;
        raceEvent.EventName = updatedEvent.EventName;
        raceEvent.Description = updatedEvent.Description;
        raceEvent.EventDate = updatedEvent.EventDate;
        raceEvent.StartTime = updatedEvent.StartTime;
        raceEvent.Location = updatedEvent.Location;
        raceEvent.Status = updatedEvent.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Events/1
    [Authorize(Roles = "Organiser")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var raceEvent = await _context.Events.FindAsync(id);

        if (raceEvent == null)
        {
            return NotFound(new
            {
                message = $"Event with ID {id} was not found."
            });
        }

        _context.Events.Remove(raceEvent);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                message = "This event cannot be deleted because related records may exist."
            });
        }

        return NoContent();
    }
}