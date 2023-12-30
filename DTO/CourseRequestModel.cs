namespace ApplyingGenericRepositoryPattern.DTO;

public class CourseRequestModel
{
    public string? CourseName { get; set; }
    public string? CourseCode { get; set; }
    public int CreditHours { get; set; }
    public int? PreRequest { get; set; }
    public int? CourseMark { get; set; }
    public int DepartmentId { get; set; }
}
