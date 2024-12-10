using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Commands;

public record DeleteAccountCommand(Guid AccountId) : IRequest<IResult>;