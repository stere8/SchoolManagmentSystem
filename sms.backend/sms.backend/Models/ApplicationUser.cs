using Microsoft.AspNetCore.Identity;

namespace sms.backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
