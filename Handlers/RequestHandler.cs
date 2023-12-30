
using ApplyingGenericRepositoryPattern.Data;

namespace ApplyingGenericRepositoryPattern.Handlers;

public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
{
    protected readonly ApplicationDbContext _context;

    protected RequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public abstract Task<TResponse> HandleAsync(TRequest request);
}
