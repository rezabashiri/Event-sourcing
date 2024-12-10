using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.User.DTOs;
using Modules.Accounting.Domain.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.User.Queries;

internal class GetUserByIdHandler(IAccountingDbContext accountingDbContext, IMapper mapper, IStringLocalizer<GetUserByIdHandler> localizer) : IRequestHandler<GetUserByIdQuery, IResult<UserDto>>
{
    public async Task<IResult<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await accountingDbContext.Users.Include(x => x.Accounts).AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return user != null ? Result<UserDto>.Success(mapper.Map<UserDto>(user)) : Result<UserDto>.Fail(localizer["User not found!"]);
    }
}