namespace ApplyingGenericRepositoryPattern.Handlers;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
    IRequestHandler<TRequest, TResponse> SetNext(IRequestHandler<TRequest, TResponse> next);
}
