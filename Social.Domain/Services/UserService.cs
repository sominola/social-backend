using Social.Domain.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Social.Data.Entities;
using Social.Data.Repositories.Interfaces;
using Social.Domain.Dto.Users;
using Social.Domain.Exceptions;
using Social.Domain.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Social.Domain.Services;

public class UserService:IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _context;
    public UserService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor context)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _context = context;
    }
    
    public async Task CreateUserAsync(RegisterUserDto model)
    {
        var emailTaken = await IsEmailTakenAsync(model.Email);
        if (emailTaken)
        {
            throw new ConflictException("Account already exists");
        }
        
        try
        {
            var user = _mapper.Map<User>(model);
            var userRole = (await _unitOfWork.RoleRepository.GetAllAsync()).FirstOrDefault();
            user.HashedPassword = BC.HashPassword(model.Password);
            user.Roles = new List<Role> {userRole};
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();
        }
        catch
        { 
            throw new AppException( "Cannot create account.");
        }
    }

    public async Task<UpdateUserDto> GetCurrentUserAsync()
    {
        var id = _context.HttpContext.User.GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        var roles = await _unitOfWork.RoleRepository.GetRolesByUserAsync(id);
        user.Roles = roles;
        return _mapper.Map<UpdateUserDto>(user);
    }
    
    
    public async Task UpdateCredentialsAsync(UpdateUserDto model)
    {
        
        var userId = _context.HttpContext.User.GetCurrentUserId();
        var isSameEmail = await _unitOfWork.UserRepository.GetEmailByAsync(userId) == model.Email;
            
        if (!isSameEmail && await IsEmailTakenAsync(model.Email))
            throw new ConflictException("This email is taken");
        
        var user = _mapper.Map<User>(model);
        user.Id = userId;
        await _unitOfWork.UserRepository.UpdateCredentialsAsync(user);
        await _unitOfWork.SaveAsync();
    }
    
    private async Task<bool> IsEmailTakenAsync(string email)
    {
        return await _unitOfWork.UserRepository.IsEmailTakenAsync(email);
    }
}