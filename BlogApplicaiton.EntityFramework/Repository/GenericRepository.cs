using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BlogApplicaiton.EntityFramework.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _table;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _table = _dbContext.Set<T>();
    }
    
    public async Task Create(T entity)
    {
        await _table.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _table.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _table.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<T?> GetById(Guid id) => await _table.FindAsync(id);

    public Task<T?> GetBy(Expression<Func<T, bool>> predicate) => _table.FirstOrDefaultAsync(predicate);

    public IQueryable<T> GetAll() => _table.AsQueryable();

    public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate) => _table.Where(predicate).AsNoTracking();
}