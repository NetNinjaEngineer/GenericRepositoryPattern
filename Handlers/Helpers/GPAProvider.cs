using ApplyingGenericRepositoryPattern.Data;

namespace ApplyingGenericRepositoryPattern.Handlers.Helpers;

public class GPAProvider(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public Task<decimal?> CalculateTotalGPA(int studentId)
    {
        decimal totalCreditHours = 0;
        decimal totalGradePoints = 0;

        var result = _context.Enrollments.Any(x => x.StudentMark == null
            && x.StudentId == studentId);

        if (!result)
        {
            var query = from e in _context.Enrollments
                        join c in _context.Courses on e.CourseId equals c.CourseId
                        where e.StudentId == studentId
                        select new
                        {
                            CourseCreditHours = c.CreditHours,
                            Mark = e.StudentMark
                        };

            foreach (var item in query)
            {
                decimal gradePoint = CalculateRatePoint(item.Mark);
                totalCreditHours += item.CourseCreditHours;
                totalGradePoints += gradePoint * item.CourseCreditHours;
            }

            decimal? gpa = totalCreditHours > 0 ? totalGradePoints / totalCreditHours : 0;

            return Task.FromResult((decimal?)Math.Round(gpa.Value, 1));
        }
        else
            return Task.FromResult(default(decimal?));
    }

    private decimal CalculateRatePoint(int? studentMark)
    {
        decimal ratePoint;
        if (studentMark >= 90 && studentMark <= 100)
            ratePoint = 4.0m;

        else if (studentMark >= 80 && studentMark < 90)
            ratePoint = 3.0m;

        else if (studentMark >= 70 && studentMark < 80)
            ratePoint = 2.0m;

        else if (studentMark >= 60 && studentMark < 70)
            ratePoint = 1.0m;

        else
            ratePoint = 0.0m;

        return ratePoint;

    }

    public IEnumerable<string> GetSuggestedCourses(int studentId)
    {
        var departmemt = (from course in _context.Courses
                          join department in _context.Departments
                          on course.DepartmentId equals department.DepartmentId
                          join enrollment in _context.Enrollments
                          on course.CourseId equals enrollment.CourseId
                          join student in _context.Students
                          on enrollment.StudentId equals student.StudentId
                          where student.StudentId == studentId
                          select department.DepartmentName).FirstOrDefault();

        var suggestedCourses = (from course in _context.Courses
                                join department in _context.Departments
                                on course.DepartmentId equals department.DepartmentId
                                join enrollment in _context.Enrollments
                                on course.CourseId equals enrollment.CourseId
                                join student in _context.Students
                                on enrollment.StudentId equals student.StudentId
                                where course.PreRequest == null && department.DepartmentName!.Equals(departmemt)
                                 && !_context.Enrollments.Any(e => e.CourseId == course.CourseId &&
                                 e.StudentId == studentId)
                                select course.CourseName).Distinct();

        return suggestedCourses.AsEnumerable();
    }

    public Task<int?> GetPreRequestId(int courseId)
    {
        var query = _context.Courses
            .FirstOrDefault(x => x.CourseId == courseId)?.PreRequest;

        return Task.FromResult(query);
    }

}
