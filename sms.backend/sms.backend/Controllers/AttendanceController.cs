using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sms.backend.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using sms.backend.Models;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController(SchoolContext context, ILogger<AttendanceController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendance()
    {
        try
        {
            _logger.LogInformation("Getting all attendance records");
            return await _context.Attendances.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all attendance records");

            return StatusCode(500, $"An error occurred while processing your attendance request.{ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Attendance>> GetAttendanceRecord(int id)
    {
        try
        {
            _logger.LogInformation("Getting attendance record with ID: {Id}", id);
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                _logger.LogWarning("Attendance record with ID: {Id} not found", id);
                return NotFound();
            }
            return attendance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the attendance record with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your attendance request.{ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Attendance>> PostAttendance(Attendance attendance)
    {
        try
        {
            _logger.LogInformation("Creating new attendance record");
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAttendanceRecord), new { id = attendance.AttendanceId }, attendance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new attendance record");
            return StatusCode(500, $"An error occurred while processing your attendance request.{ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAttendance(int id, Attendance attendance)
    {
        try
        {
            if (id != attendance.AttendanceId)
            {
                return BadRequest();
            }
            _context.Entry(attendance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the attendance record with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        try
        {
            _logger.LogInformation("Deleting attendance record with ID: {Id}", id);
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the attendance record with ID: {Id}", id);
            return StatusCode(500, $"An error occurred while processing your attendance request.{ex.Message}");
        }
    }
}