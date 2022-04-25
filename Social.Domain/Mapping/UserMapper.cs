using AutoMapper;
using Social.Data.Entities;
using Social.Domain.Dto.Users;

namespace Social.Domain.Mapping;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<RegisterUserDto, User>().ReverseMap();
        CreateMap<RegisterUserDto, LoginDto>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<User, UpdateUserDto>()
            .ForMember(x => x.Roles, y => y.MapFrom(x => x.Roles));
        CreateMap<UpdateUserDto, User>()
            .ForMember(x => x.Roles, y => y.Ignore());
    }
}