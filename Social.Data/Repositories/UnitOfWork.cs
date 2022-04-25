using Social.Data.Context;
using Social.Data.Repositories.Interfaces;

namespace Social.Data.Repositories;

public class UnitOfWork:IUnitOfWork
{
    private readonly AppDbContext _db;
    private IUserRepository _userRepository;
    private IRoleRepository _roleRepository;
    public UnitOfWork(AppDbContext applicationContext)
    {
        _db = applicationContext;
    }

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_db);
    public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_db);

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

}