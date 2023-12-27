
using ApplyingGenericRepositoryPattern.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApplyingGenericRepositoryPattern.Repository;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
{
    public readonly ApplicationDbContext _context = context;

    public async Task<T> Create(T entity)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Delete(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var entities = _context.Set<T>();
        return await entities.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Expression<Func<T, bool>> condition)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(condition);
        return entity!;
    }

    public async Task<T> Update(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
