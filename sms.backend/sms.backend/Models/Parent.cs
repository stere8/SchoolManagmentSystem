namespace sms.backend.Models
{
    public class Parent
    {
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Student> Children { get; set; }
    }
}
