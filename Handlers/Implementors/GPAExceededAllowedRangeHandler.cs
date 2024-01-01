using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public sealed class GPAExceededAllowedRangeHandler(ApplicationDbContext context, GPAProvider gpaProvider)
    : RequestHandler<SuggestCoursesRequest, SuggestCoursesResponse>(context)
{
    private readonly GPAProvider _gpaProvider = gpaProvider;
    public override async Task<SuggestCoursesResponse> HandleAsync(SuggestCoursesRequest request)
    {
        var gpa = await _gpaProvider.CalculateTotalGPA(request.StudentId);
        if (gpa.HasValue && gpa > 2.5m)
        {
            return new SuggestCoursesResponse()
            {
                Message = "Your gpa in safe side",
                SuggestionCourses = Enumerable.Empty<string>()
            };
        }

        return new SuggestCoursesResponse()
        {
            Message = "Your GPA is't in allowed Range",
            SuggestionCourses = Enumerable.Empty<string>()
        };
    }

    public override IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse>
        SetNext(IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse> next)
    {
        nextHandler = next;
        return this;
    }
}