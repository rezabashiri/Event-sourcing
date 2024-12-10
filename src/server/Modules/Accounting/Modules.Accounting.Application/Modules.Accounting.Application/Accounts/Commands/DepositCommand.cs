using MediatR;
using Modules.Accounting.Application.Accounts.DTOs;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Commands;

public record DepositCommand(Guid AccountId, decimal Amount) : IRequest<IResult<AccountDTo>>;