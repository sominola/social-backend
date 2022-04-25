namespace Social.Data.Repositories.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
    Task SaveAsync();
}