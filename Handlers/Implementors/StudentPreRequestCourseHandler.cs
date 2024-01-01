using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public class StudentPreRequestCourseHandler(ApplicationDbContext context)
    : RequestHandler<CoursePreRequestRequest, CoursePreRequestResponse>(context)
{
    public override async Task<CoursePreRequestResponse> HandleAsync(CoursePreRequestRequest request)
    {
        int? preRequisteId = null;
        var query = _context.Courses
            .FirstOrDefault(x => x.CourseId == request.CourseId)?.PreRequest;

        preRequisteId = query;

        if (preRequisteId == null)
            return new CoursePreRequestResponse()
            {
                Course = string.Empty,
                TakenCourse = true
            };

        else
            return await nextHandler!.HandleAsync(request);

    }

    public override IRequestHandler<CoursePreRequestRequest, CoursePreRequestResponse>
        SetNext(IRequestHandler<CoursePreRequestRequest, CoursePreRequestResponse> next)
    {
        nextHandler = next;
        return this;
    }
}
