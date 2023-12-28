using ApplyingGenericRepositoryPattern.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApplyingGenericRepositoryPattern.Repository.Implementation;

public class GenericRepository<T>(ApplicationDbContext context)
    : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context = context;
    private DbSet<T> _dbSet = context.Set<T>();


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

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ToListAsync();
    }

    private string GetPrimaryKeyName(Type type)
    {
        var entityType = _context.Model.FindEntityType(type);
        return entityType.FindPrimaryKey().Properties.FirstOrDefault()?.Name;
    }



    public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        var primaryKeyName = GetPrimaryKeyName(typeof(T));

        var entity = await query.SingleAsync(e => EF.Property<int>(e, primaryKeyName) == id);

        return entity;
    }


    public async Task<T> Update(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
