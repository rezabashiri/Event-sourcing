using AutoMapper;
using Modules.Accounting.Application.User.DTOs;

namespace Modules.Accounting.Application.User.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Domain.Entities.User, UserDto>()
            .ConstructUsing(src => new UserDto(src.Id, src.FullName, src.Accounts.Count));
    }
}