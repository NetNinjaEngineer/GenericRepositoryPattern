using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public class StudentHasEnrollmentHandler(ApplicationDbContext context, GPAProvider gpaProvider)
    : RequestHandler<CoursePreRequestRequest, CoursePreRequestResponse>(context)
{
    private readonly GPAProvider _gpaProvider = gpaProvider;
    public override async Task<CoursePreRequestResponse> HandleAsync(CoursePreRequestRequest request)
    {
        var preRequisteId = await _gpaProvider.GetPreRequestId(request.CourseId);

        var checkHasEnrollment = await _context.Enrollments.FirstOrDefaultAsync(x =>
             x.CourseId == preRequisteId.Value && x.StudentId == request.StudentId);

        if (checkHasEnrollment != null)
            return new CoursePreRequestResponse()
            {
                TakenCourse = true,
                Course = string.Empty
            };

        else
            return new CoursePreRequestResponse()
            {
                TakenCourse = false,
                Course = _context.Courses.FirstOrDefault(x
                    => x.CourseId == preRequisteId)!.CourseName!
            };
    }

    public override IRequestHandler<CoursePreRequestRequest, CoursePreRequestResponse>
        SetNext(IRequestHandler<CoursePreRequestRequest, CoursePreRequestResponse> next)
    {
        nextHandler = next;
        return this;
    }
}
