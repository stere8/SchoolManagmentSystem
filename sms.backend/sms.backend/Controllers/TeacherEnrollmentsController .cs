using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TeacherEnrollmentsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<TeacherEnrollmentsController> _logger;

    public TeacherEnrollmentsController(SchoolContext context, ILogger<TeacherEnrollmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<TeacherEnrollmentsViews>>> GetAllTeacherEnrollments()
    {
        try
        {
            _logger.LogInformation("Getting all teacher enrollments");
            var teacherEnrollments = await _context.TeacherEnrollments.ToListAsync();
            var staff = await _context.Staff.ToListAsync();
            var classes = await _context.Classes.ToListAsync();

            var returnedViewsList = new List<TeacherEnrollmentsViews>();

            foreach (TeacherEnrollment enroll in teacherEnrollments)
            {
                var teacher = staff.FirstOrDefault(s => s.StaffId == enroll.StaffId);
                var classItem = classes.FirstOrDefault(c => c.ClassId == enroll.ClassId);

                if (teacher != null && classItem != null)
                {
                    var teacherName = $"{teacher.FirstName} {teacher.LastName}";
                    returnedViewsList.Add(new TeacherEnrollmentsViews()
                    {
                        EnrollmentRef = enroll.TeacherEnrollmentId,
                        AssignedClass = classItem.Name,
                        EnrolledTeacher = teacherName,
                        AssignedLesson = teacher.SubjectExpertise
                    });
                }
            }

            _logger.LogInformation("Successfully retrieved teacher enrollments.");
            return Ok(returnedViewsList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all teacher enrollments");
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherEnrollment>> GetTeacherEnrollmentById(int id)
    {
        try
        {
            _logger.LogInformation("Getting teacher enrollment with ID: {Id}", id);
            var teacherEnrollment = await _context.TeacherEnrollments.FindAsync(id);
            if (teacherEnrollment == null)
            {
                _logger.LogWarning("Teacher Enrollment with ID: {Id} not found", id);
                return NotFound();
            }

            return teacherEnrollment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the teacher enrollment with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpGet("{teacherId}/{classId}")]
    public async Task<ActionResult<TeacherEnrollment>> GetTeacherEnrollment(int teacherId, int classId)
    {
        try
        {
            _logger.LogInformation("Getting teacher enrollment for Teacher ID: {TeacherId} and Class ID: {ClassId}", teacherId, classId);
            var teacherEnrollment = await _context.TeacherEnrollments.FindAsync(teacherId, classId);
            if (teacherEnrollment == null)
            {
                _logger.LogWarning("Teacher Enrollment for Teacher ID: {TeacherId} and Class ID: {ClassId} not found", teacherId, classId);
                return NotFound();
            }

            return teacherEnrollment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the teacher enrollment for Teacher ID: {TeacherId} and Class ID: {ClassId}", teacherId, classId);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<TeacherEnrollment>> PostTeacherEnrollment(TeacherEnrollment teacherEnrollment)
    {
        try
        {
            _logger.LogInformation("Creating new teacher enrollment");
            _context.TeacherEnrollments.Add(teacherEnrollment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeacherEnrollment), new { teacherId = teacherEnrollment.StaffId, classId = teacherEnrollment.ClassId }, teacherEnrollment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new teacher enrollment");
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpDelete("{teacherId}/{classId}")]
    public async Task<IActionResult> DeleteTeacherEnrollment(int teacherId, int classId)
    {
        try
        {
            _logger.LogInformation("Deleting teacher enrollment for Teacher ID: {TeacherId} and Class ID: {ClassId}", teacherId, classId);
            var teacherEnrollment = await _context.TeacherEnrollments.FindAsync(teacherId, classId);
            if (teacherEnrollment == null)
            {
                return NotFound();
            }

            _context.TeacherEnrollments.Remove(teacherEnrollment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the teacher enrollment for Teacher ID: {TeacherId} and Class ID: {ClassId}", teacherId, classId);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacherEnrollment(int id, TeacherEnrollment updatedEnrollment)
    {
        try
        {
            if (id != updatedEnrollment.TeacherEnrollmentId)
            {
                return BadRequest("TeacherEnrollmentID mismatch.");
            }

            _context.Entry(updatedEnrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TeacherEnrollments.Any(e => e.TeacherEnrollmentId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the teacher enrollment with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }
}