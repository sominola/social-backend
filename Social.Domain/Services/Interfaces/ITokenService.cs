using System.Security.Claims;

namespace Social.Domain.Services.Interfaces;

public interface ITokenService
{
   string GenerateJwt(IEnumerable<Claim> claims);
}