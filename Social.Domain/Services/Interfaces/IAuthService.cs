using Social.Domain.Dto;
using Social.Domain.Dto.Users;

namespace Social.Domain.Services.Interfaces;

public interface IAuthService
{
    Task<TokenDto> Login(LoginDto model);
}