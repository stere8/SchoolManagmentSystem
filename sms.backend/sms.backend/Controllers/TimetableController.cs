using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sms.backend.Views;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimetablesController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<TimetablesController> _logger;

        public TimetablesController(SchoolContext context, ILogger<TimetablesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timetable>>> GetTimetables()
        {
            try
            {
                _logger.LogInformation("Getting all timetables");
                return await _context.Timetables.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all timetables");
                return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Timetable>> GetTimetable(int id)
        {
            try
            {
                _logger.LogInformation("Getting timetable with ID: {Id}", id);
                var timetable = await _context.Timetables.FindAsync(id);
                if (timetable == null)
                {
                    _logger.LogWarning("Timetable with ID: {Id} not found", id);
                    return NotFound();
                }
                return timetable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the timetable with ID: {Id}", id);
                return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Timetable>> PostTimetable(Timetable timetable)
        {
            try
            {
                _logger.LogInformation("Creating new timetable");
                _context.Timetables.Add(timetable);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTimetable), new { id = timetable.TimetableId }, timetable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new timetable");
                return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimetable(int id, Timetable timetable)
        {
            try
            {
                _logger.LogInformation("Updating timetable with ID: {Id}", id);
                if (id != timetable.TimetableId)
                {
                    return BadRequest();
                }
                _context.Entry(timetable).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the timetable with ID: {Id}", id);
                return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimetable(int id)
        {
            try
            {
                _logger.LogInformation("Deleting timetable with ID: {Id}", id);
                var timetable = await _context.Timetables.FindAsync(id);
                if (timetable == null)
                {
                    return NotFound();
                }
                _context.Timetables.Remove(timetable);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the timetable with ID: {Id}", id);
                return StatusCode(500, $"An error occurred while processing your request.{ex.Message}");
            }
        }
    }
}