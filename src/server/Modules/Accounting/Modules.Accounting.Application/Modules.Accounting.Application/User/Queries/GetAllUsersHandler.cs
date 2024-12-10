using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.User.DTOs;
using Modules.Accounting.Domain.Abstractions;
using Shared.Core.Extensions;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.User.Queries;

public class GetAllUsersHandler(IAccountingDbContext dbContext, IMapper mapper, IStringLocalizer<GetAllUsersHandler> localizer) : IRequestHandler<GetAllUsers, PaginatedResult<UserDto>>
{
    public async Task<PaginatedResult<UserDto>> Handle(GetAllUsers request, CancellationToken cancellationToken)
    {
        var paginatedResult = await dbContext.Users.Include(x => x.Accounts).AsNoTracking().ToPaginatedListAsync(1, 100);

        if (paginatedResult is { TotalCount: > 0 })
        {
            var userDtos = paginatedResult.Data.Select(mapper.Map<UserDto>).ToList();
            return PaginatedResult<UserDto>.Success(userDtos, paginatedResult.TotalCount, paginatedResult.CurrentPage, paginatedResult.PageSize);
        }

        return PaginatedResult<UserDto>.Fail(localizer["User not found!"]);
    }
}