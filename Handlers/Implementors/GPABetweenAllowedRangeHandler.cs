using ApplyingGenericRepositoryPattern.Handlers.Helpers;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public sealed class GPABetweenAllowedRangeHandler : IRequestHandler<SuggestCoursesRequest, bool>
{
    private readonly StudentGPAHandler _studentGPAHandler;

    public GPABetweenAllowedRangeHandler(StudentGPAHandler studentGPAHandler)
    {
        _studentGPAHandler = studentGPAHandler;
    }

    public async Task<bool> HandleAsync(SuggestCoursesRequest request)
    {
        var gpa = await _studentGPAHandler.HandleAsync(request);
        if (gpa >= 1.8m && gpa <= 2.5m)
            return true;
        return false;
    }
}