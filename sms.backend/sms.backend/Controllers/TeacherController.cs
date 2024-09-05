using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using sms.backend.Views;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<TeachersController> _logger;

        public TeachersController(SchoolContext context, ILogger<TeachersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}/classes")]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses(int id)
        {
            try
            {
                _logger.LogInformation("Getting classes for teacher with ID: {Id}", id);
                var teacher = await _context.Teachers.Include(t => t.Classes).FirstOrDefaultAsync(t => t.TeacherId == id);
                if (teacher == null)
                {
                    _logger.LogWarning("Teacher with ID: {Id} not found", id);
                    return NotFound();
                }
                return Ok(teacher.Classes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting classes for teacher with ID: {Id}", id);
                return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
            }
        }

        [HttpPost("{id}/classes")]
        public async Task<ActionResult<Class>> AddClass(int id, Class classItem)
        {
            try
            {
                _logger.LogInformation("Adding class for teacher with ID: {Id}", id);
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher == null)
                {
                    _logger.LogWarning("Teacher with ID: {Id} not found", id);
                    return NotFound();
                }
                teacher.Classes.Add(classItem);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetClasses), new { id = teacher.TeacherId }, classItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding class for teacher with ID: {Id}", id);
                return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
            }
        }
    }
}