
using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public class StudentExistenceHandler(ApplicationDbContext context)
    : RequestHandler<SuggestCoursesRequest, bool>(context)
{
    public override async Task<bool> HandleAsync(SuggestCoursesRequest request)
    {
        var existStudent = await _context.Students.AnyAsync(
          x => x.StudentId == request.StudentId);
        return existStudent;
    }
}
