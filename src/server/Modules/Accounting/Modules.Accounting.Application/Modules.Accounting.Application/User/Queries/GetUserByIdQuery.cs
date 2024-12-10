using MediatR;
using Modules.Accounting.Application.User.DTOs;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.User.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<IResult<UserDto>>;