using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public CategoriesController(RaceDayDbContext context)
    {
        _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        return Ok(categories);
    }

    // GET: api/Categories/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return NotFound(new
            {
                message = $"Category with ID {id} was not found."
            });
        }

        return Ok(category);
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(
        Category category)
    {
        category.CategoryId = 0;

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.CategoryId },
            category);
    }

    // PUT: api/Categories/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(
        int id, Category updatedCategory)
    {
        if (id != updatedCategory.CategoryId)
        {
            return BadRequest(new
            {
                message = "The URL ID must match the category ID in the request."
            });
        }

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound(new
            {
                message = $"Category with ID {id} was not found."
            });
        }

        category.CategoryName = updatedCategory.CategoryName;
        category.Description = updatedCategory.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Categories/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound(new
            {
                message = $"Category with ID {id} was not found."
            });
        }

        _context.Categories.Remove(category);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                message = "This category cannot be deleted because related event categories may exist."
            });
        }

        return NoContent();
    }
}