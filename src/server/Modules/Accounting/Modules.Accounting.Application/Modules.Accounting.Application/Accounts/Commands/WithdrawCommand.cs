using MediatR;
using Modules.Accounting.Application.Accounts.DTOs;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Commands;

public record WithdrawCommand(Guid AccountId, decimal Amount) : IRequest<IResult<AccountDTo>>;