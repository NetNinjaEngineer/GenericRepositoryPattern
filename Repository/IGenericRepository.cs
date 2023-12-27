using System.Linq.Expressions;

namespace ApplyingGenericRepositoryPattern.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Expression<Func<T, bool>> condition);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(T entity);
}
