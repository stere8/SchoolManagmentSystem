using sms.backend.Models;

public class Student
{
    public int StudentId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<Class> Classes { get; set; }
}