using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.DTO;
using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;
using ApplyingGenericRepositoryPattern.Handlers.Implementors;
using ApplyingGenericRepositoryPattern.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplyingGenericRepositoryPattern.Repository.Implementation;

public class FeatureService(ApplicationDbContext context, IMapper mapper) : IFeatureService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<(string, string)> AssignCourseToStudent(int courseId, int studentId)
    {
        var courseName = _context.Courses
            .FirstOrDefault(x => x.CourseId == courseId)?.CourseName;

        var studentName = string.Concat(_context.Students
            .FirstOrDefault(x => x.StudentId == studentId)?.FirstName, ' ', _context.Students
            .FirstOrDefault(x => x.StudentId == studentId)?.FirstName
        );

        await _context.Enrollments
           .AddAsync(new Enrollment() { StudentId = studentId, CourseId = courseId });

        await _context.SaveChangesAsync();

        return ValueTuple.Create(courseName, studentName)!;

    }

    public async Task<bool> CheckCourseHaveBeenEnrolled(int courseId, int studentId)
    {
        var enrolledCourse = await _context.Enrollments.FirstOrDefaultAsync(x =>
            x.CourseId == courseId && x.StudentId == studentId);

        if (enrolledCourse is not null)
            return true;

        return false;
    }

    public async Task<(bool, string)> CheckPreRequestCourse(int courseId, int studentId)
    {
        var gpaProvider = new GPAProvider(_context);
        var studentPreRequestCourseHandler = new StudentPreRequestCourseHandler(_context);
        var studentHasEnrollmentHandler = new StudentHasEnrollmentHandler(_context, gpaProvider);
        studentPreRequestCourseHandler.SetNext(studentHasEnrollmentHandler);
        var request = new CoursePreRequestRequest() { CourseId = courseId, StudentId = studentId };
        var response = await studentPreRequestCourseHandler.HandleAsync(request);
        return ValueTuple.Create(response.TakenCourse!, response.Course!);
    }

    public async Task<bool> CheckValidIDS(int courseId, int studentId)
    {
        var validStudentId = await _context.Students.AnyAsync(x => x.StudentId == studentId);
        var validCourseId = await _context.Courses.AnyAsync(x => x.CourseId == courseId);

        if (!validCourseId || !validStudentId)
            return false;

        return true;

    }

    public async Task<IEnumerable<CoursesWithDepartmentsModel>> GetCoursesWithDepartments()
    {
        var coursesWithDepartments = await _context.CoursesWithDepartments.ToListAsync();
        return coursesWithDepartments;
    }

    public async Task<IEnumerable<CoursesWithDepartmentsAndPreRequestsModel>> GetCoursesWithDepartmentsAndPreRequests()
    {
        var coursesWithDepartmentsAndPreRequests =
            await _context.CoursesWithDepartmentsAndPreRequests.ToListAsync();

        return coursesWithDepartmentsAndPreRequests;
    }

    public async Task<IEnumerable<CoursesWithPreRequestsModel>> GetCoursesWithPreRequests()
    {
        var coursesWithPreRequests = await _context.CoursesWithPreRequests.ToListAsync();
        return coursesWithPreRequests;
    }

    public async Task<IEnumerable<ShowStudentsWithCoursesRegisteredModel>> GetStudentsWithCoursesRegistered()
    {
        var studentsWithCoursesRegistered =
            await _context.StudentsWithCoursesRegistered.ToListAsync();

        return studentsWithCoursesRegistered;
    }

    public async Task<Enrollment> UpdateEnrollment(Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<Enrollment> GetEnrollmentById(int studentId, int courseId)
    {
        var enrollement = await _context.Enrollments.SingleOrDefaultAsync(
            x => x.StudentId == studentId && x.CourseId == courseId);

        if (enrollement is not null)
            return enrollement;

        return default!;
    }

    public async Task<(string, IEnumerable<string>)> SuggestCoursesDependOnDepartments(int studentId)
    {
        var provider = new GPAProvider(context);

        var studentExistenceAndEnrolledHandler = new StudentExistenceAndEnrolledHandler(context);
        var studentGpaExistenceHandler = new StudentGpaExistenceHandler(context, provider);
        var gpaBetweenAllowedRangeHandler = new GPABetweenAllowedRangeHandler(context, provider);
        var gpaExceededAllowedRangeHandler = new GPAExceededAllowedRangeHandler(context, provider);

        studentExistenceAndEnrolledHandler.SetNext(studentGpaExistenceHandler);
        studentGpaExistenceHandler.SetNext(gpaBetweenAllowedRangeHandler);
        gpaBetweenAllowedRangeHandler.SetNext(gpaExceededAllowedRangeHandler);

        var suggestCoursesRequest = new SuggestCoursesRequest() { StudentId = studentId };
        var response = await studentExistenceAndEnrolledHandler.HandleAsync(suggestCoursesRequest);
        return ValueTuple.Create(response.Message!, response.SuggestionCourses!);

    }

    public async Task<Student> DeleteStudent(int studentId)
    {
        var student = await _context.Students.Include(s => s.Enrollments)
            .FirstOrDefaultAsync(student => student.StudentId == studentId);

        if (student == null)
            return default!;

        _context.Students.Remove(student);
        _context.SaveChanges();

        return student;
    }

    public async Task<Student> GetStudentById(int studentId)
    {
        var student = await _context.Students.SingleOrDefaultAsync(student =>
            student.StudentId == studentId);

        if (student is not null)
            return student;

        return default!;
    }

    public Task<IEnumerable<Enrollment>> GetEnrollmentsBy(int studentId)
    {
        var enrollments = _context.Enrollments.Where(x => x.StudentId == studentId);

        if (enrollments is not null)
            return Task.FromResult(enrollments.AsEnumerable());

        return Task.FromResult(Enumerable.Empty<Enrollment>());
    }

    public async Task<IQueryable<string>> GetEnrolledCoursesFor(int studentId)
    {
        var validStudent = await GetStudentById(studentId);
        if (validStudent == null)
            return Enumerable.Empty<string>().AsQueryable();

        var enrolledCourses = _context.Courses
            .Join(_context.Departments,
                course => course.DepartmentId,
                department => department.DepartmentId,
                (course, department) => new { course, department })
            .Join(_context.Enrollments,
                cdept => cdept.course.CourseId,
                enrollment => enrollment.CourseId,
                (cdept, enrollment) => new { cdept.course, cdept.department, enrollment })
            .Join(_context.Students,
                cde => cde.enrollment.StudentId,
                student => student.StudentId,
                (cde, student) => new { cde.course, cde.department, cde.enrollment, student })
            .Where(x => x.student.StudentId == studentId &&
                _context.Enrollments.Any(e =>
                    e.StudentId == x.student.StudentId && e.CourseId == x.course.CourseId))
            .GroupBy(
                x => string.Concat(x.student.FirstName, " ", x.student.LastName),
                (key, grouped) => new EnrolledCourseDTO()
                {
                    Student = key,
                    Courses = string.Join(", ", grouped.Select(x => x.course.CourseName))
                });


        return enrolledCourses.Select(x => x.Courses!);

    }

    public async Task<int?> GetEnrollmentsCount(int studentId)
    {
        var existStudent = await GetStudentById(studentId);
        if (existStudent is null)
            return null;

        var enrolledCoursesCount = _context.Enrollments.Count(e => e.StudentId == studentId);
        return enrolledCoursesCount;
    }

    public IEnumerable<DepartmentsWithCourses> GetDepartmentsWithCourses()
    {
        var departmentsWithCourses = _context.Departments
            .Join(_context.Courses,
                department => department.DepartmentId,
                course => course.DepartmentId,
                (department, course) => new { department, course })
            .GroupBy(
                key => key.department.DepartmentName,
                (key, grouped) => new DepartmentsWithCourses()
                {
                    Department = key,
                    Courses = _mapper.Map<IEnumerable<MappingCourse>>(grouped.Select(x => x.course).ToList())
                });

        return departmentsWithCourses;

    }
}
