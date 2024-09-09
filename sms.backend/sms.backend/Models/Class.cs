using sms.backend.Models;

public class Class
{
    public int ClassId { get; set; }
    public string Name { get; set; }
    public int GradeLevel { get; set; }
    public int Year { get; set; }
    public int? TeacherId { get; set; } // Ensure this property exists
}
