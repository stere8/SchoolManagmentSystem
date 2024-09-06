using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(SchoolContext context, ILogger<StudentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        try
        {
            _logger.LogInformation("Getting all students");
            return await _context.Students.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all students");
            return StatusCode(500, $"An error occurred while processing your student request.{ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        try
        {
            _logger.LogInformation("Getting student with ID: {Id}", id);
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found", id);
                return NotFound();
            }
            return student;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the student with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your student request.{ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Student>> PostStudent(Student student)
    {
        try
        {
            _logger.LogInformation("Creating new student");
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new student");
            return StatusCode(500, $"An error occurred while processing your student request.{ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, Student student)
    {
        try
        {
            _logger.LogInformation("Updating student with ID: {Id}", id);
            if (id != student.StudentId)
            {
                return BadRequest();
            }
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the student with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your student request.{ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        try
        {
            _logger.LogInformation("Deleting student with ID: {Id}", id);
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the student with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your student request.{ex.Message}");
        }
    }
}