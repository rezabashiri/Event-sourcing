using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.User.Commands;

public record CreateUserCommand(string FullName) : IRequest<IResult<Guid>>;