namespace sms.backend.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Class> Classes { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SubjectExpertise { get; set; }
    }
}