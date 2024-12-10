using MediatR;
using Modules.Accounting.Application.Accounts.DTOs;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Queries;

public record GetUserAccountsByUserIdQuery(Guid UserId) : IRequest<IResult<UserAccountsDto>>;