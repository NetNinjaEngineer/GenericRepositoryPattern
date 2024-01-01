
using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public sealed class StudentExistenceAndEnrolledHandler(ApplicationDbContext context)
    : RequestHandler<SuggestCoursesRequest, SuggestCoursesResponse>(context)
{
    public override async Task<SuggestCoursesResponse> HandleAsync(SuggestCoursesRequest request)
    {
        var existStudent = await _context.Students.AnyAsync(x => x.StudentId == request.StudentId);
        var enrolled = await _context.Enrollments.AnyAsync(e => e.StudentId == request.StudentId);
        if (existStudent && enrolled)
        {
            return await nextHandler!.HandleAsync(request);
        }

        return new SuggestCoursesResponse
        {
            Message = "",
            SuggestionCourses = Enumerable.Empty<string>()
        };
    }

    public override IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse> SetNext(IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse> next)
    {
        nextHandler = next;
        return this;
    }
}
