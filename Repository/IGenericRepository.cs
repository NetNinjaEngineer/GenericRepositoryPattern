using System.Linq.Expressions;

namespace ApplyingGenericRepositoryPattern.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task<T> GetByIdAsync(int id,
        params Expression<Func<T, object>>[] includes);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(T entity);
}
