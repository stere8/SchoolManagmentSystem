using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Views;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<EnrollmentsController> _logger;

    public EnrollmentsController(SchoolContext context, ILogger<EnrollmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<EnrollmentsViews>>> GetAllEnrollments()
    {
        try
        {
            _logger.LogInformation("Getting all enrollments");
            var enrollments = await _context.Enrollments.ToListAsync();
            var students = await _context.Students.ToListAsync();
            var classes = await _context.Classes.ToListAsync();

            var returnedViewsList = new List<EnrollmentsViews>();

            foreach (var enroll in enrollments)
            {
                var student = students.FirstOrDefault(s => s.StudentId == enroll.StudentId);
                var classItem = classes.FirstOrDefault(c => c.ClassId == enroll.ClassId);

                if (student != null && classItem != null)
                {
                    var studentName = $"{student.FirstName} {student.LastName}";
                    returnedViewsList.Add(new EnrollmentsViews()
                    {
                        EnrollmentRef = enroll.EnrollmentId,
                        EnrolledClass = classItem.Name,
                        EnrolledStudent = studentName
                    });
                }
            }

            _logger.LogInformation("Successfully retrieved enrollments.");
            return Ok(returnedViewsList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all enrollments");
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Enrollment>> GetEnrollmentById(int id)
    {
        try
        {
            _logger.LogInformation("Getting enrollment with ID: {Id}", id);
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(enrol => enrol.EnrollmentId == id);
            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment with ID: {Id} not found", id);
                return NotFound();
            }

            return enrollment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the enrollment with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpGet("{studentId}/{classId}")]
    public async Task<ActionResult<Enrollment>> GetEnrollment(int studentId, int classId)
    {
        try
        {
            _logger.LogInformation("Getting enrollment for Student ID: {StudentId} and Class ID: {ClassId}", studentId, classId);
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId && e.ClassId == classId);
            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment for Student ID: {StudentId} and Class ID: {ClassId} not found", studentId, classId);
                return NotFound();
            }

            return enrollment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the enrollment for Student ID: {StudentId} and Class ID: {ClassId}", studentId, classId);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Enrollment>> PostEnrollment(EnrollmentInsert enrollmentInsert)
    {
        try
        {
            _logger.LogInformation("Creating new enrollment");

            Enrollment enrollment = new Enrollment()
            {
                ClassId = enrollmentInsert.ClassId,
                StudentId = enrollmentInsert.StudentId
            };
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEnrollment), new { studentId = enrollment.StudentId, classId = enrollment.ClassId }, enrollment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new enrollment");
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnrollment(int id, Enrollment updatedEnrollment)
    {
        try
        {
            if (id != updatedEnrollment.EnrollmentId)
            {
                return BadRequest("EnrollmentID mismatch.");
            }

            var existingEnrollment = await _context.Enrollments.FindAsync(id);
            if (existingEnrollment == null)
            {
                return NotFound();
            }

            // Update fields except EnrollmentID
            existingEnrollment.StudentId = updatedEnrollment.StudentId;
            existingEnrollment.ClassId = updatedEnrollment.ClassId;
            // Update other fields as necessary

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!_context.Enrollments.Any(e => e.EnrollmentId == id))
            {
                return NotFound();
            }
            else
            {
                _logger.LogError(ex, "A concurrency error occurred while updating the enrollment with ID: {Id}", id);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the enrollment with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpDelete("{studentId}/{classId}")]
    public async Task<IActionResult> DeleteEnrollment(int studentId, int classId)
    {
        try
        {
            _logger.LogInformation("Deleting enrollment for Student ID: {StudentId} and Class ID: {ClassId}", studentId, classId);
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId && e.ClassId == classId);
            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the enrollment for Student ID: {StudentId} and Class ID: {ClassId}", studentId, classId);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }
}