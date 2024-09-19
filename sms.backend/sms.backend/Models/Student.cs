namespace sms.backend.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? GradeLevel { get; set; }
        public string? ParentContactInfo { get; set; }
        public int? ParentId { get; set; }
    }
}