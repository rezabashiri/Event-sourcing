using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.Accounts.DTOs;
using Modules.Accounting.Domain.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Queries;

public class GetUserAccountsByUserIdHandler(IAccountingDbContext accountingDbContext, IMapper mapper, IStringLocalizer<GetUserAccountsByUserIdHandler> localizer)
    : IRequestHandler<GetUserAccountsByUserIdQuery, IResult<UserAccountsDto>>
{
    public async Task<IResult<UserAccountsDto>> Handle(GetUserAccountsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var accounts = await accountingDbContext.Accounts.AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken: cancellationToken);

        return accounts.Count > 0
            ? Result<UserAccountsDto>.Success(new UserAccountsDto(request.UserId, accounts.ConvertAll(mapper.Map<AccountDTo>)))
            : Result<UserAccountsDto>.Fail(localizer["User not found!"]);
    }
}