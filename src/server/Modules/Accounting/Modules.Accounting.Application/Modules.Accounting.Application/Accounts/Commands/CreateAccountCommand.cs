using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Commands;

public record CreateAccountCommand(Guid UserId, string Name, decimal InitialDeposit) : IRequest<IResult>;