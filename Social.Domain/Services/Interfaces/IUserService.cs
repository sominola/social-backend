using Social.Domain.Dto.Users;

namespace Social.Domain.Services.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(RegisterUserDto model);
    Task<UpdateUserDto> GetCurrentUserAsync();
    Task UpdateCredentialsAsync(UpdateUserDto model);
}