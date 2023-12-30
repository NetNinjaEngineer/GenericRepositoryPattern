namespace ApplyingGenericRepositoryPattern.Handlers;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
}
