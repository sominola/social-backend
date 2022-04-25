using BC = BCrypt.Net.BCrypt;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Social.Data.Entities;
using Social.Data.Repositories.Interfaces;
using Social.Domain.Dto;
using Social.Domain.Dto.Users;
using Social.Domain.Exceptions;
using Social.Domain.Services.Interfaces;

namespace Social.Domain.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenDto> Login(LoginDto model)
    {
        var result = await VerifyUserCredentials(model);
        var claims = GenerateClaims(result);
        var token = _tokenService.GenerateJwt(claims);
        return new TokenDto {AccessToken = token};
    }

    private IEnumerable<Claim> GenerateClaims(User user)
    {
        var claims = new List<Claim>()
        {
            new("id", user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim("role", role.Name));
        }

        return claims;
    }

    private async Task<User> VerifyUserCredentials(LoginDto model)
    {
        var user = await _unitOfWork.UserRepository.GetCredentialUserAsync(model.Email);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }


        var isVerified = BC.Verify(model.Password, user!.HashedPassword);
        if (!isVerified)
        {
            throw new ForbiddenException("Password doesn't fit");
        }

        return user;
    }
}