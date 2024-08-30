using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using sms.backend.Views;

namespace sms.backend.Controllers
{
    // Path: /sms.backend/sms.backend/Controllers/TeacherController.cs
    // Path: /sms.backend/sms.backend/Controllers/TeachersController.cs
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
            var teacher = await _context.Teachers.Include(t => t.Classes).FirstOrDefaultAsync(t => t.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher.Classes);
        }

        [HttpPost("{id}/classes")]
        public async Task<ActionResult<Class>> AddClass(int id, Class classItem)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            teacher.Classes.Add(classItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClasses), new { id = teacher.TeacherId }, classItem);
        }
    }
}
