using ApplyingGenericRepositoryPattern.Handlers.Helpers;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public class GPAExceededAllowedRangeHandler : IRequestHandler<SuggestCoursesRequest, bool>
{
    private readonly StudentGPAHandler _studentGPAHandler;

    public GPAExceededAllowedRangeHandler(StudentGPAHandler studentGPAHandler)
    {
        _studentGPAHandler = studentGPAHandler;
    }

    public async Task<bool> HandleAsync(SuggestCoursesRequest request)
    {
        var gpa = await _studentGPAHandler.HandleAsync(request);
        if (gpa > 2.5m)
            return true;
        return false;
    }
}
