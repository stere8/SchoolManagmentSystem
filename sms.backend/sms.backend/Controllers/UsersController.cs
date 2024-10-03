using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SchoolContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(SchoolContext context, ILogger<UsersController> logger, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Getting all users");
                var users = await _userManager.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users");
                return StatusCode(500, $"An error occurred while processing your user request: {ex.Message}");
            }
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers(string role)

        {
            try
            {
                _logger.LogInformation("Getting all users");
                var users = await _userManager.Users.Where(user => user.Role == role).ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users");
                return StatusCode(500, $"An error occurred while processing your user request: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            try
            {
                _logger.LogInformation("Getting user with ID: {Id}", id);
                var user = await _userManager.FindByIdAsync(id);
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

        public async Task<IActionResult> AssignUserToEntity([FromBody] UserAssignmentRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.EntityId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                switch (user.Role)
                {
                    case "teacher":
                        var teacher = await _context.Staff.FindAsync(request.UserId);
                        if (teacher == null)
                        {
                            return NotFound("Teacher not found");
                        }
                        break;
                    case "student":
                        var student = await _context.Students.FindAsync(request.UserId);
                        if (student == null)
                        {
                            return NotFound("Student not found");
                        }
                        break;
                    default:
                        return BadRequest("Invalid role for assignment");
                }

                // Check if the user or entity already exists in the Users table
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId || u.EntityId == request.EntityId);
                if (existingUser != null)
                {
                    return BadRequest("User or entity is already assigned");
                }

                User newUser = new User()
                {
                    EntityId = request.EntityId,
                    UserId = request.UserId
                };
                _context.Users.Add(newUser);
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
        //[Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    return NotFound("User not found");
                }
                var userEntity = await _userManager.FindByIdAsync(userId);
                var user = await _context.Users.FirstAsync(u => u.EntityId == userEntity.Id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var classesID = new List<int>();

                if (userEntity.Role.ToLower() == "teacher")
                {
                    classesID = await _context.TeacherEnrollments
                        .Where(e => e.StaffId == user.UserId)
                        .Select(e => e.ClassId)
                        .ToListAsync();
                }
                else if (userEntity.Role.ToLower() == "student")
                {
                    classesID = await _context.Enrollments
                        .Where(e => e.StudentId == user.UserId)
                        .Select(e => e.ClassId)
                        .ToListAsync();
                }

                var classes = await _context.Classes.Where(c => classesID.Contains(c.ClassId)).ToListAsync();

                var userInfo = new
                {
                    userEntity.UserName,
                    userEntity.Email,
                    userEntity.Role,
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
        public string EntityId { get; set; }
    }

    public class UserInfoView
    {
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}