using ApplyingGenericRepositoryPattern.Handlers.Helpers;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public class SuggestCoursesHandler : IRequestHandler<SuggestCoursesRequest, SuggestCoursesResponse>
{
    private readonly StudentExistenceHandler _studentExistenceHandler;
    private readonly StudentEnrolledHandler _studentEnrolledHandler;
    private readonly StudentGPAHandler _studentGPAHandler;
    private readonly GPAProvider _gpaProvider;
    private readonly GPABetweenAllowedRangeHandler _gpaBetweenAllowedRangeHandler;
    private readonly GPAExceededAllowedRangeHandler _gpaExceededAllowedRangeHandler;

    public SuggestCoursesHandler(StudentExistenceHandler studentExistenceHandler, StudentEnrolledHandler studentEnrolledHandler, StudentGPAHandler studentGPAHandler, GPAProvider gpaProvider, GPABetweenAllowedRangeHandler gpaBetweenAllowedRangeHandler, GPAExceededAllowedRangeHandler gpaExceededAllowedRangeHandler)
    {
        _studentExistenceHandler = studentExistenceHandler;
        _studentEnrolledHandler = studentEnrolledHandler;
        _studentGPAHandler = studentGPAHandler;
        _gpaProvider = gpaProvider;
        _gpaBetweenAllowedRangeHandler = gpaBetweenAllowedRangeHandler;
        _gpaExceededAllowedRangeHandler = gpaExceededAllowedRangeHandler;
    }

    public async Task<SuggestCoursesResponse> HandleAsync(SuggestCoursesRequest request)
    {
        var existStudent = await _studentExistenceHandler.HandleAsync(request);
        var enrolledStudent = await _studentEnrolledHandler.HandleAsync(request);
        var gpa = await _studentGPAHandler.HandleAsync(request);
        var gpaBetweenAllowedRange = await _gpaBetweenAllowedRangeHandler.HandleAsync(request);
        var gpaExceededAllowedRange = await _gpaExceededAllowedRangeHandler.HandleAsync(request);

        if (existStudent && enrolledStudent)
        {
            if (gpa.HasValue)
            {
                if (gpaBetweenAllowedRange)
                {
                    var suggestedCourses = _gpaProvider.GetSuggestedCourses(request.StudentId);
                    return new SuggestCoursesResponse()
                    {
                        Message = "Suggested Courses: ",
                        SuggestionCourses = suggestedCourses
                    };
                }
                else if (gpaExceededAllowedRange)
                    return new SuggestCoursesResponse()
                    {
                        Message = "GPA in safe side",
                        SuggestionCourses = Enumerable.Empty<string>()
                    };
                else
                    return new SuggestCoursesResponse()
                    {
                        Message = "GPA is not in allowed ranges to suggest courses",
                        SuggestionCourses = Enumerable.Empty<string>()
                    };
            }
        }
        return new SuggestCoursesResponse
        {
            Message = "Student may be not exist or not enrolled",
            SuggestionCourses = Enumerable.Empty<string>()
        };
    }

}