using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

[ApiController]
[Route("[controller]")]
public class LessonsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<LessonsController> _logger;

    public LessonsController(SchoolContext context, ILogger<LessonsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons()
    {
        try
        {
            _logger.LogInformation("Getting all lessons");
            return await _context.Lessons.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all lessons");
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Lesson>> GetLesson(int id)
    {
        try
        {
            _logger.LogInformation("Getting lesson with ID: {Id}", id);
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                _logger.LogWarning("Lesson with ID: {Id} not found", id);
                return NotFound();
            }
            return lesson;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the lesson with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Lesson>> PostLesson(Lesson lesson)
    {
        try
        {
            _logger.LogInformation("Creating new lesson");
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLesson), new { id = lesson.LessonId }, lesson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new lesson");
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutLesson(int id, Lesson lesson)
    {
        try
        {
            if (id != lesson.LessonId)
            {
                return BadRequest();
            }
            _context.Entry(lesson).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the lesson with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        try
        {
            _logger.LogInformation("Deleting lesson with ID: {Id}", id);
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the lesson with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }
}