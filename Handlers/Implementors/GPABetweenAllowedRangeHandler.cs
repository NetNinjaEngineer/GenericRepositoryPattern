using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public sealed class GPABetweenAllowedRangeHandler(ApplicationDbContext context, GPAProvider gpaProvider)
    : RequestHandler<SuggestCoursesRequest, SuggestCoursesResponse>(context)
{
    private readonly GPAProvider _gpaProvider = gpaProvider;
    public override async Task<SuggestCoursesResponse> HandleAsync(SuggestCoursesRequest request)
    {
        var gpa = await _gpaProvider.CalculateTotalGPA(request.StudentId);
        if (gpa.HasValue && gpa >= 1.8m && gpa <= 2.5m)
        {
            var suggestedCourses = _gpaProvider.GetSuggestedCourses(request.StudentId);
            return new SuggestCoursesResponse()
            {
                Message = "Suggested Courses: ",
                SuggestionCourses = suggestedCourses
            };
        }

        return await nextHandler!.HandleAsync(request);
    }

    public override IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse>
        SetNext(IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse> next)
    {
        nextHandler = next;
        return this;
    }
}