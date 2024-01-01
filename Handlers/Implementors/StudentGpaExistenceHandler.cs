using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;
public sealed class StudentGpaExistenceHandler(ApplicationDbContext context, GPAProvider gpaProvider)
    : RequestHandler<SuggestCoursesRequest, SuggestCoursesResponse>(context)
{
    private readonly GPAProvider _gpaProvider = gpaProvider;
    public override async Task<SuggestCoursesResponse> HandleAsync(SuggestCoursesRequest request)
    {
        var totalGPA = await _gpaProvider.CalculateTotalGPA(request.StudentId);
        if (totalGPA.HasValue)
        {
            var res = await nextHandler!.HandleAsync(request);
            return res;
        }
        else
            return new SuggestCoursesResponse
            {
                Message = "Gpa Un available yet.",
                SuggestionCourses = Enumerable.Empty<string>()
            };
    }

    public override IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse> SetNext(IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse> next)
    {
        nextHandler = next;
        return this;
    }
}