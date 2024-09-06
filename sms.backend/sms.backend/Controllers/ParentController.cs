using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using sms.backend.Views;

[ApiController]
[Route("[controller]")]
public class ParentsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<ParentsController> _logger;

    public ParentsController(SchoolContext context, ILogger<ParentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id}/children")]
    public async Task<ActionResult<IEnumerable<Student>>> GetChildren(int id)
    {
        try
        {
            var parent = await _context.Parents.Include(p => p.Children).FirstOrDefaultAsync(p => p.ParentId == id);
            if (parent == null)
            {
                return NotFound();
            }
            return Ok(parent.Children);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting children for parent with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your parent request.{ex.Message}");
        }
    }

    [HttpPost("{id}/children")]
    public async Task<ActionResult<Student>> AddChild(int id, Student student)
    {
        try
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }
            parent.Children.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetChildren), new { id = parent.ParentId }, student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a child for parent with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your parent request.{ex.Message}");
        }
    }
}