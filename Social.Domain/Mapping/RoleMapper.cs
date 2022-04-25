using AutoMapper;
using Social.Data.Entities;

namespace Social.Domain.Mapping;

public class RoleMapper:Profile
{
    public RoleMapper()
    {
        CreateMap<Role, string>().ConvertUsing(x=>x.Name);
    }
}