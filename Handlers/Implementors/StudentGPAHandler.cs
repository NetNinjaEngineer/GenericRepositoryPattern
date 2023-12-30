using ApplyingGenericRepositoryPattern.Handlers.Helpers;

namespace ApplyingGenericRepositoryPattern.Handlers.Implementors;

public class StudentGPAHandler : IRequestHandler<SuggestCoursesRequest, decimal?>
{
    private readonly GPAProvider _gpaProvider;

    public StudentGPAHandler(GPAProvider gpaProvider)
    {
        _gpaProvider = gpaProvider;
    }

    public async Task<decimal?> HandleAsync(SuggestCoursesRequest request)
    {
        var totalGPA = await _gpaProvider.CalculateTotalGPA(request.StudentId);
        return totalGPA;
    }
}