using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventCategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventCategoriesController(RaceDayDbContext context)
    {
        _context = context;
    }

    // GET: api/EventCategories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventCategory>>> GetEventCategories()
    {
        var eventCategories = await _context.EventCategories
            .AsNoTracking()
            .ToListAsync();

        return Ok(eventCategories);
    }

    // GET: api/EventCategories/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventCategory>> GetEventCategory(int id)
    {
        var eventCategory = await _context.EventCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(ec => ec.EventCategoryId == id);

        if (eventCategory == null)
        {
            return NotFound(new
            {
                message = $"Event category with ID {id} was not found."
            });
        }

        return Ok(eventCategory);
    }

    // POST: api/EventCategories
    [HttpPost]
    public async Task<ActionResult<EventCategory>> CreateEventCategory(
        EventCategory eventCategory)
    {
        var eventExists = await _context.Events
            .AnyAsync(e => e.EventId == eventCategory.EventId);

        if (!eventExists)
        {
            return BadRequest(new
            {
                message = "The specified event does not exist."
            });
        }

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.CategoryId == eventCategory.CategoryId);

        if (!categoryExists)
        {
            return BadRequest(new
            {
                message = "The specified category does not exist."
            });
        }

        eventCategory.EventCategoryId = 0;

        _context.EventCategories.Add(eventCategory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEventCategory),
            new { id = eventCategory.EventCategoryId },
            eventCategory);
    }

    // PUT: api/EventCategories/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEventCategory(
        int id, EventCategory updatedEventCategory)
    {
        if (id != updatedEventCategory.EventCategoryId)
        {
            return BadRequest(new
            {
                message = "The URL ID must match the event category ID."
            });
        }

        var eventCategory = await _context.EventCategories.FindAsync(id);

        if (eventCategory == null)
        {
            return NotFound(new
            {
                message = $"Event category with ID {id} was not found."
            });
        }

        var eventExists = await _context.Events
            .AnyAsync(e => e.EventId == updatedEventCategory.EventId);

        if (!eventExists)
        {
            return BadRequest(new
            {
                message = "The specified event does not exist."
            });
        }

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.CategoryId == updatedEventCategory.CategoryId);

        if (!categoryExists)
        {
            return BadRequest(new
            {
                message = "The specified category does not exist."
            });
        }

        eventCategory.EventId = updatedEventCategory.EventId;
        eventCategory.CategoryId = updatedEventCategory.CategoryId;
        eventCategory.EntryFee = updatedEventCategory.EntryFee;
        eventCategory.MaximumParticipants =
            updatedEventCategory.MaximumParticipants;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/EventCategories/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEventCategory(int id)
    {
        var eventCategory = await _context.EventCategories.FindAsync(id);

        if (eventCategory == null)
        {
            return NotFound(new
            {
                message = $"Event category with ID {id} was not found."
            });
        }

        _context.EventCategories.Remove(eventCategory);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                message = "This event category cannot be deleted because related enrolments may exist."
            });
        }

        return NoContent();
    }
}