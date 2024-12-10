using AutoMapper;
using Modules.Accounting.Application.Accounts.DTOs;
using Modules.Accounting.Domain.Entities;

namespace Modules.Accounting.Application.Accounts.Mappings;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Account, AccountDTo>()
            .ConstructUsing(src => new AccountDTo(src.Id, src.Name, src.Balance));
    }
}