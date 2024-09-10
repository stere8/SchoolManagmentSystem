    // /sms.backend/sms.backend/Controllers/UsersController.cs
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using sms.backend.Data;
    using sms.backend.Models;
    using System.Security.Claims;

    namespace sms.backend.Controllers
    {
        [Authorize(Roles = "Admin,Teacher,Student,Parent")]
        [ApiController]
        [Route("[controller]")]
        public class UsersController : ControllerBase
        {
            private readonly SchoolContext _context;
            private readonly ILogger<UsersController> _logger;

            public UsersController(SchoolContext context, ILogger<UsersController> logger)
            {
                _context = context;
                _logger = logger;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<User>>> GetUsers()
            {
                try
                {
                    _logger.LogInformation("Getting all users");
                    return await _context.Users.ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while getting all users");
                    return StatusCode(500, $"An error occurred while processing your user request.{ex.Message}");
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<User>> GetUser(int id)
            {
                try
                {
                    _logger.LogInformation("Getting user with ID: {Id}", id);
                    var user = await _context.Users.FindAsync(id);
                    if (user == null)
                    {
                        _logger.LogWarning("User with ID: {Id} not found", id);
                        return NotFound();
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while getting the user with ID: {Id}", id);
                    return StatusCode(500, $"An error occurred while processing your user request.{ex.Message}");
                }
            }

            [HttpPost]
            public async Task<ActionResult<User>> PostUser(User user)
            {
                try
                {
                    _logger.LogInformation("Creating new user");
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating a new user");
                    return StatusCode(500, $"An error occurred while processing your user request.{ex.Message}");
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutUser(int id, User user)
            {
                try
                {
                    _logger.LogInformation("Updating user with ID: {Id}", id);
                    if (id != user.UserId)
                    {
                        return BadRequest();
                    }
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the user with ID: {Id}", id);
                    return StatusCode(500, $"An error occurred while processing your user request.{ex.Message}");
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                try
                {
                    _logger.LogInformation("Deleting user with ID: {Id}", id);
                    var user = await _context.Users.FindAsync(id);
                    if (user == null)
                    {
                        _logger.LogWarning("User with ID: {Id} not found", id);
                        return NotFound();
                    }
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while deleting the user with ID: {Id}", id);
                    return StatusCode(500, $"An error occurred while processing your user request.{ex.Message}");
                }
            }

            [HttpPost("assign")]
            public async Task<IActionResult> AssignUserToEntity([FromBody] UserAssignmentRequest request)
            {
                try
                {
                    var user = await _context.Users.FindAsync(request.UserId);
                    if (user == null)
                    {
                        return NotFound("User not found");
                    }

                    switch (user.Role)
                    {
                        case UserRole.Teacher:
                            var teacher = await _context.Teachers.FindAsync(request.EntityId);
                            if (teacher == null)
                            {
                                return NotFound("Teacher not found");
                            }
                            user.TeacherId = teacher.TeacherId;
                            teacher.UserId = user.UserId;
                            break;
                        case UserRole.Student:
                            var student = await _context.Students.FindAsync(request.EntityId);
                            if (student == null)
                            {
                                return NotFound("Student not found");
                            }
                            user.StudentId = student.StudentId;
                            student.UserId = user.UserId;
                            break;
                        default:
                            return BadRequest("Invalid role for assignment");
                    }

                    await _context.SaveChangesAsync();
                    return Ok("User assigned successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while assigning the user");
                    return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
                }
            }

            [HttpGet("current")]
            [Authorize]
            public async Task<IActionResult> GetCurrentUserInfo()
            {
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var user = await _context.Users
                        .Include(u => u.Teacher)
                        .Include(u => u.Student)
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    if (user == null)
                    {
                        return NotFound("User not found");
                    }

                    var classesID = new List<int>();

                    if (user.Role == UserRole.Teacher && user.Teacher != null)
                    {
                        classesID = await _context.TeacherEnrollments
                            .Where(e => e.StaffId == user.Teacher.TeacherId)
                            .Select(e => e.ClassId)
                            .ToListAsync();
                    }
                    else if (user.Role == UserRole.Student && user.Student != null)
                    {
                        classesID = await _context.Enrollments
                            .Where(e => e.StudentId == user.Student.StudentId)
                            .Select(e => e.ClassId)
                            .ToListAsync();

                    }

                    var classes = await _context.Classes.Where(c => classesID.Contains(c.ClassId)).ToListAsync();


                    var userInfo = new
                    {
                        user.Username,
                        user.Email,
                        user.Role,
                        Classes = classes.Select(c => new { c.ClassId, c.Name })
                    };

                    return Ok(userInfo);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while getting current user info");
                    return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
                }
            }
        }

        public class UserAssignmentRequest
        {
            public int UserId { get; set; }
            public int EntityId { get; set; }
        }
    }