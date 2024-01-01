namespace ApplyingGenericRepositoryPattern.Handlers.Helpers;

public class SuggestCoursesResponse
{
    public string? Message { get; set; }
    public IEnumerable<string>? SuggestionCourses { get; set; } = [];
}
