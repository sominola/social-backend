using Social.Data.Entities;

namespace Social.Data.Repositories.Interfaces;

public interface IRoleRepository:IRepository<Role>
{
    Task<List<Role>> GetRolesByUserAsync(long userId);
}