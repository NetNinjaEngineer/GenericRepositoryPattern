namespace ApplyingGenericRepositoryPattern.Helpers;

public class CoursesWithDepartmentsAndPreRequestsModel : BaseEntity
{
    public string? PreRequest { get; set; }
    public string? DepartmentAbbreviation { get; set; }
}
