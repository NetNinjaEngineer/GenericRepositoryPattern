
using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public class StudentEnrolledHandler(ApplicationDbContext context)
    : RequestHandler<SuggestCoursesRequest, bool>(context)
{
    public override async Task<bool> HandleAsync(SuggestCoursesRequest request)
    {
        var enrolled = await _context.Enrollments.AnyAsync(e => e.StudentId == request.StudentId);
        return enrolled;
    }
}
