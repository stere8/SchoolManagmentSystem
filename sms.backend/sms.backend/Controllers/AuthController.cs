using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sms.backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using sms.backend.Data;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly SchoolContext _context;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration, ILogger<AuthController> logger, SchoolContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { errors = new List<string> { "Email is already in use" } });
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Role = model.Role };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "User registered successfully" });
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { errors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while registering a new user");
            return StatusCode(500, $"An error occurred while processing your auth request.{ex.Message}");
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = GenerateJwtToken(user);

                if (user.Role.ToLower() == "admin")
                {
                    return Ok(new { token, role = user.Role });
                }

                // Find the linked user entity in the "Users" table
                var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.EntityId == user.Id);

                if (userEntity != null)
                {
                    object userDetails = null;

                    switch (user.Role.ToLower())
                    {
                        case "teacher":
                            var teacher = await _context.Staff
                                .FirstOrDefaultAsync(t => t.StaffId == userEntity.UserId);

                            if (teacher != null)
                            {
                                var assignedClasses = await _context.TeacherEnrollments
                                    .Where(te => te.StaffId == teacher.StaffId)                                    .ToListAsync();

                                userDetails = new
                                {
                                    teacher.StaffId,
                                    teacher.FirstName,
                                    teacher.LastName,
                                    teacher.SubjectExpertise,
                                    AssignedClasses = assignedClasses
                                };
                            }
                            break;

                        case "student":
                            var student = await _context.Students
                                .FirstOrDefaultAsync(s => s.StudentId == userEntity.UserId);

                            if (student != null)
                            {
                                var enrolledClasses = await _context.Enrollments
                                    .Where(e => e.StudentId == student.StudentId)
                                    .Join(_context.Classes,
                                        e => e.ClassId,
                                        c => c.ClassId,
                                        (e, c) => new
                                        {
                                            ClassId = c.ClassId,
                                            ClassName = c.Name
                                        })
                                    .ToListAsync();

                                userDetails = new
                                {
                                    student.StudentId,
                                    student.FirstName,
                                    student.LastName,
                                    student.GradeLevel,
                                    EnrolledClasses = enrolledClasses
                                };
                            }
                            break;

                        case "parent":
                            var parent = await _context.Parents
                                .FirstOrDefaultAsync(p => p.ParentId == userEntity.UserId);

                            if (parent != null)
                            {
                                var children = await _context.ParentChildAssignments
                                    .Where(pca => pca.ParentId == parent.ParentId)
                                    .Join(_context.Students,
                                        pca => pca.ChildId,
                                        s => s.StudentId,
                                        (pca, s) => new
                                        {
                                            s.StudentId,
                                            s.FirstName,
                                            s.LastName,
                                            s.GradeLevel
                                        })
                                    .ToListAsync();

                                userDetails = new
                                {
                                    parent.ParentId,
                                    parent.FirstName,
                                    parent.LastName,
                                    Children = children
                                };
                            }
                            break;
                    }

                    if (userDetails != null)
                    {
                        return Ok(new { token, role = user.Role, details = userDetails });
                    }
                    else
                    {
                        return NotFound("User entity details could not be found");
                    }
                }

                return Unauthorized("User does not exist in the Users table");
            }

            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging in");
            return StatusCode(500, $"An error occurred while processing your auth request.{ex.Message}");
        }
    }



    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

        var token = new JwtSecurityToken(
            _configuration["JwtIssuer"],
            _configuration["JwtIssuer"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}