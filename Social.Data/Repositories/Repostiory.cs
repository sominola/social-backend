using Microsoft.EntityFrameworkCore;
using Social.Data.Context;
using Social.Data.Entities;
using Social.Data.Repositories.Interfaces;

namespace Social.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Db;
    private readonly DbSet<T> _entities;

    public Repository(AppDbContext db)
    {
        Db = db;
        _entities = Db.Set<T>();
    }

    public IQueryable<T> GetQueryableNoTracking()
    {
        return _entities.AsNoTracking();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(long id)
    {
        return await _entities.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _entities.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _entities.AddRangeAsync(entities);
    }
    
    public async Task UpdateAsync(T entity)
    { 
        await Task.Run(() => _entities.Update(entity).Entity);
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        await Task.Run(() => entities.ToList().ForEach(item => Db.Entry(item).State = EntityState.Modified));
    }
    
    public async Task RemoveAsync(long id)
    {
        var found = await _entities.FirstOrDefaultAsync(entity => entity.Id == id);

        if (found != null)
        {
            _entities.Remove(found);
        }
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        await Task.Run(() => entities.ToList().ForEach(item => Db.Entry(item).State = EntityState.Deleted));
    }
    
    public async Task<bool> IsEntityExistAsync(long id)
    {
        return await _entities.AnyAsync(x => x.Id == id);
    }


  

  
}