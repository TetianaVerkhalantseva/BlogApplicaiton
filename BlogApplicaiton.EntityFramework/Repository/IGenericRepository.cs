using System.Linq.Expressions;

namespace BlogApplicaiton.EntityFramework.Repository;

public interface IGenericRepository<T> where T : class
{
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    
    Task<T?> GetById(Guid id);
    Task<T?> GetBy(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetAll();
    IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
}