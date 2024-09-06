using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<StaffController> _logger;

    public StaffController(SchoolContext context, ILogger<StaffController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Staff>>> GetStaff()
    {
        try
        {
            _logger.LogInformation("Getting all staff members");
            return await _context.Staff.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all staff members");
            return StatusCode(500, $"An error occurred while processing your staff request.{ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Staff>> GetStaff(int id)
    {
        try
        {
            _logger.LogInformation("Getting staff member with ID: {Id}", id);
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                _logger.LogWarning("Staff member with ID: {Id} not found", id);
                return NotFound();
            }
            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the staff member with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your staff request.{ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Staff>> PostStaff(Staff staff)
    {
        try
        {
            _logger.LogInformation("Creating new staff member");
            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStaff), new { id = staff.StaffId }, staff);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new staff member");
            return StatusCode(500, $"An error occurred while processing your staff request.{ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutStaff(int id, Staff staff)
    {
        try
        {
            _logger.LogInformation("Updating staff member with ID: {Id}", id);
            if (id != staff.StaffId)
            {
                return BadRequest();
            }
            _context.Entry(staff).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the staff member with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your staff request.{ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        try
        {
            _logger.LogInformation("Deleting staff member with ID: {Id}", id);
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the staff member with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your staff request.{ex.Message}");
        }
    }
}