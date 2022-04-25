using Social.Data.Entities;

namespace Social.Data.Repositories.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> GetQueryableNoTracking();

    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(long id);

    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    Task UpdateAsync(T entity);

    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task RemoveAsync(long id);
    Task RemoveRangeAsync(IEnumerable<T> entities);

    Task<bool> IsEntityExistAsync(long id);
}