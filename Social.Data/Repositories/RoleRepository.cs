using Microsoft.EntityFrameworkCore;
using Social.Data.Context;
using Social.Data.Entities;
using Social.Data.Repositories.Interfaces;

namespace Social.Data.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<List<Role>> GetRolesByUserAsync(long userId)
    {
        return await GetQueryableNoTracking().Include(x => x.Users).Where(x => x.Users.Any(user => user.Id == userId))
            .ToListAsync();
    }
}