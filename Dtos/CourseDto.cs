namespace ApplyingGenericRepositoryPattern.Dtos;

public class CourseDto
{
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? CourseCode { get; set; }
    public int CreditHours { get; set; }
    public int? PreRequest { get; set; }
    public int? CourseMark { get; set; }
    public string? Department { get; set; }
}
