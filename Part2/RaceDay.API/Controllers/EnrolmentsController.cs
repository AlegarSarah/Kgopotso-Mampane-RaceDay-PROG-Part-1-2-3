using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EnrolmentsController(RaceDayDbContext context)
    {
        _context = context;
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        return int.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out userId);
    }

    // GET: api/Enrolments
    // Organisers see all enrolments; participants see only their own.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments()
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized();

        var query = _context.Enrolments.AsNoTracking();

        if (User.IsInRole("Participant"))
        {
            query = query.Where(e => e.ParticipantId == userId);
        }
        else if (!User.IsInRole("Organiser"))
        {
            return Forbid();
        }

        return Ok(await query.ToListAsync());
    }

    // GET: api/Enrolments/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized();

        var enrolment = await _context.Enrolments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EnrolmentId == id);

        if (enrolment == null)
        {
            return NotFound(new
            {
                message = $"Enrolment with ID {id} was not found."
            });
        }

        if (User.IsInRole("Participant") &&
            enrolment.ParticipantId != userId)
        {
            return Forbid();
        }

        if (!User.IsInRole("Participant") &&
            !User.IsInRole("Organiser"))
        {
            return Forbid();
        }

        return Ok(enrolment);
    }

    // POST: api/Enrolments
    [HttpPost]
    [Authorize(Roles = "Organiser,Participant")]
    public async Task<ActionResult<Enrolment>> CreateEnrolment(
        Enrolment enrolment)
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized();

        // Participants can create enrolments only for themselves.
        if (User.IsInRole("Participant"))
        {
            enrolment.ParticipantId = userId;
        }

        var participantExists = await _context.Users
            .AnyAsync(u => u.UserId == enrolment.ParticipantId);

        if (!participantExists)
        {
            return BadRequest(new
            {
                message = "The specified participant does not exist."
            });
        }

        var eventCategoryExists = await _context.EventCategories
            .AnyAsync(ec =>
                ec.EventCategoryId == enrolment.EventCategoryId);

        if (!eventCategoryExists)
        {
            return BadRequest(new
            {
                message = "The specified event category does not exist."
            });
        }

        enrolment.EnrolmentId = 0;

        _context.Enrolments.Add(enrolment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEnrolment),
            new { id = enrolment.EnrolmentId },
            enrolment);
    }

    // PUT: api/Enrolments/1
    // Only organisers can update enrolment records.
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateEnrolment(
        int id, Enrolment updatedEnrolment)
    {
        if (id != updatedEnrolment.EnrolmentId)
        {
            return BadRequest(new
            {
                message = "The URL ID must match the enrolment ID."
            });
        }

        var enrolment = await _context.Enrolments.FindAsync(id);

        if (enrolment == null)
        {
            return NotFound(new
            {
                message = $"Enrolment with ID {id} was not found."
            });
        }

        var participantExists = await _context.Users
            .AnyAsync(u =>
                u.UserId == updatedEnrolment.ParticipantId &&
                u.Role == "Participant");

        if (!participantExists)
        {
            return BadRequest(new
            {
                message = "The specified participant does not exist or is not a participant."
            });
        }

        var eventCategoryExists = await _context.EventCategories
            .AnyAsync(ec =>
                ec.EventCategoryId == updatedEnrolment.EventCategoryId);

        if (!eventCategoryExists)
        {
            return BadRequest(new
            {
                message = "The specified event category does not exist."
            });
        }

        enrolment.ParticipantId = updatedEnrolment.ParticipantId;
        enrolment.EventCategoryId = updatedEnrolment.EventCategoryId;
        enrolment.EnrolmentDate = updatedEnrolment.EnrolmentDate;
        enrolment.EnrolmentStatus = updatedEnrolment.EnrolmentStatus;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Enrolments/1
    // Organisers can delete any enrolment; participants only their own.
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser,Participant")]
    public async Task<IActionResult> DeleteEnrolment(int id)
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized();

        var enrolment = await _context.Enrolments.FindAsync(id);

        if (enrolment == null)
        {
            return NotFound(new
            {
                message = $"Enrolment with ID {id} was not found."
            });
        }

        if (User.IsInRole("Participant") &&
            enrolment.ParticipantId != userId)
        {
            return Forbid();
        }

        if (!User.IsInRole("Participant") &&
            !User.IsInRole("Organiser"))
        {
            return Forbid();
        }

        _context.Enrolments.Remove(enrolment);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                message = "This enrolment cannot be deleted because related results may exist."
            });
        }

        return NoContent();
    }
}