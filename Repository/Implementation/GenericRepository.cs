
using ApplyingGenericRepositoryPattern.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApplyingGenericRepositoryPattern.Repository.Implementation;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<T> DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
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

    public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;
        foreach (var include in includes)
            query = query.Include(include);
        var primaryKeyName = GetPrimaryKeyName(typeof(T));
        var entity = await query.SingleOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
        return entity!;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    private string GetPrimaryKeyName(Type type)
    {
        var entityType = _context.Model.FindEntityType(type);
        return entityType.FindPrimaryKey().Properties.FirstOrDefault()?.Name;
    }
}
