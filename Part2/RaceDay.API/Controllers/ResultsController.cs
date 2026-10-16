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
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ResultsController(RaceDayDbContext context)
    {
        _context = context;
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        return int.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out userId);
    }

    // GET: api/Results
    // Organisers see all results; participants see only their own.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Result>>> GetResults()
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized();

        var query = _context.Results.AsNoTracking();

        if (User.IsInRole("Participant"))
        {
            query = query.Where(r =>
                r.Enrolment != null &&
                r.Enrolment.ParticipantId == userId);
        }
        else if (!User.IsInRole("Organiser"))
        {
            return Forbid();
        }

        return Ok(await query.ToListAsync());
    }

    // GET: api/Results/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Result>> GetResult(int id)
    {
        if (!TryGetCurrentUserId(out int userId))
            return Unauthorized();

        var result = await _context.Results
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ResultId == id);

        if (result == null)
        {
            return NotFound(new
            {
                message = $"Result with ID {id} was not found."
            });
        }

        if (User.IsInRole("Participant"))
        {
            var belongsToParticipant = await _context.Enrolments
                .AnyAsync(e =>
                    e.EnrolmentId == result.EnrolmentId &&
                    e.ParticipantId == userId);

            if (!belongsToParticipant)
                return Forbid();
        }
        else if (!User.IsInRole("Organiser"))
        {
            return Forbid();
        }

        return Ok(result);
    }

    // POST: api/Results
    // Organisers only.
    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<ActionResult<Result>> CreateResult(Result result)
    {
        var enrolmentExists = await _context.Enrolments
            .AnyAsync(e => e.EnrolmentId == result.EnrolmentId);

        if (!enrolmentExists)
        {
            return BadRequest(new
            {
                message = "The specified enrolment does not exist."
            });
        }

        result.ResultId = 0;

        _context.Results.Add(result);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetResult),
            new { id = result.ResultId },
            result);
    }

    // PUT: api/Results/1
    // Organisers only.
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateResult(
        int id, Result updatedResult)
    {
        if (id != updatedResult.ResultId)
        {
            return BadRequest(new
            {
                message = "The URL ID must match the result ID."
            });
        }

        var result = await _context.Results.FindAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                message = $"Result with ID {id} was not found."
            });
        }

        var enrolmentExists = await _context.Enrolments
            .AnyAsync(e => e.EnrolmentId == updatedResult.EnrolmentId);

        if (!enrolmentExists)
        {
            return BadRequest(new
            {
                message = "The specified enrolment does not exist."
            });
        }

        result.EnrolmentId = updatedResult.EnrolmentId;
        result.FinishTime = updatedResult.FinishTime;
        result.Position = updatedResult.Position;
        result.ResultStatus = updatedResult.ResultStatus;
        result.RecordDate = updatedResult.RecordDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Results/1
    // Organisers only.
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteResult(int id)
    {
        var result = await _context.Results.FindAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                message = $"Result with ID {id} was not found."
            });
        }

        _context.Results.Remove(result);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}