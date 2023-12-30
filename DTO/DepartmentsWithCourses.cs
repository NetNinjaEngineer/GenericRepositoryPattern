namespace ApplyingGenericRepositoryPattern.DTO;

public class DepartmentsWithCourses
{
    public string? Department { get; set; }
    public IEnumerable<MappingCourse> Courses { get; set; } = [];
}
