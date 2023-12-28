using System.Linq.Expressions;

namespace ApplyingGenericRepositoryPattern.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task CreateAsync(T entity);
    Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
    Task<bool> SaveChangesAsync();
}
