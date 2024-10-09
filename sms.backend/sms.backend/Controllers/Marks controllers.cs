using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

[ApiController]
[Route("[controller]")]
public class MarksController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<MarksController> _logger;

    public MarksController(SchoolContext context, ILogger<MarksController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mark>>> GetMarks()
    {
        try
        {
            _logger.LogInformation("Getting all marks");
            return await _context.Marks.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all marks");
            return StatusCode(500, $"An error occurred while processing your marks request.{ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Mark>> GetMark(int id)
    {
        try
        {
            _logger.LogInformation("Getting mark with ID: {Id}", id);
            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                _logger.LogWarning("Mark with ID: {Id} not found", id);
                return NotFound();
            }
            return mark;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the mark with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your marks request.{ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Mark>> PostMark(Mark mark)
    {
        try
        {
            _logger.LogInformation("Creating new mark");
            _context.Marks.Add(mark);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMark), new { id = mark.MarkId }, mark);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new mark");
            return StatusCode(500, $"An error occurred while processing your marks request.{ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMark(int id, Mark mark)
    {
        try
        {
            if (id != mark.MarkId)
            {
                return BadRequest();
            }
            _context.Entry(mark).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the mark with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your marks request.{ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMark(int id)
    {
        try
        {
            _logger.LogInformation("Deleting mark with ID: {Id}", id);
            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            _context.Marks.Remove(mark);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the mark with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your marks request.{ex.Message}");
        }
    }

    public class MarkView
    {
        public string Student { get; set; }
        public decimal MarkDecimal { get; set; }
    }
}