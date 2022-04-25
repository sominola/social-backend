using Social.Domain.Dto;
using Social.Domain.Dto.Users;
using Social.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Social.Web.Controllers;

[ApiController]
[Route("api")]
public class AccountController: ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AccountController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }
    
    /// <summary>
    /// Login to account
    /// </summary>
    /// <remarks>Description</remarks>
    /// <returns>JWT</returns>
    /// <response code="200">Success</response>
    /// <response code="403">Error, password doesn't fit</response>
    /// <response code="404">Error, can not find account</response>
    [HttpPost("signin")]
    public async Task<TokenDto> SignIn([FromBody] LoginDto model)
    {
        return await _authService.Login(model);
    }
    
    /// <summary>
    /// Signup account
    /// </summary>
    /// <remarks>Description</remarks>
    /// <returns>Account credentials</returns>
    /// <response code="200">Success</response>
    /// <response code="409">Error, account already exists</response>
    /// <response code="500">Error, cannot create account.</response>
    [HttpPost("signup")]
    public async Task<object> SignUp([FromBody] RegisterUserDto model)
    {
        await _userService.CreateUserAsync(model);
        return Ok(model);
    }
}