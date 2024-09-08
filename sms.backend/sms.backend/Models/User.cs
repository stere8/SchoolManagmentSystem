using Microsoft.AspNetCore.Identity;

namespace sms.backend.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public int? TeacherId { get; set; }
        public int? StudentId { get; set; }
        public int? ParentId { get; set; } // Add ParentId
        public virtual Teacher Teacher { get; set; }
        public virtual Student Student { get; set; }
        public virtual Parent Parent { get; set; } // Add Parent
    }
}