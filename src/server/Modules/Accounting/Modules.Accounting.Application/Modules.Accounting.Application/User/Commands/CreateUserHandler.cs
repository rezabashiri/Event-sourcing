using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Domain.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.User.Commands;

internal class CreateUserHandler : IRequestHandler<CreateUserCommand, IResult<Guid>>
{
    private readonly IAccountingDbContext _accountingDbContext;
    private readonly IStringLocalizer<CreateUserHandler> _localizer;

    public CreateUserHandler(IAccountingDbContext accountingDbContext, IStringLocalizer<CreateUserHandler> localizer)
    {
        _accountingDbContext = accountingDbContext;
        _localizer = localizer;
    }

    public async Task<IResult<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountingDbContext.Users.FirstOrDefaultAsync(x => request.FullName.Equals(x.FullName), cancellationToken);
        if (user != null)
        {
            return Result<Guid>.Fail(_localizer["User already exists!"]);
        }

        var entity = Domain.Entities.User.Create(request.FullName);
        var toAddUser = _accountingDbContext.Users.Add(entity);
        await _accountingDbContext.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(toAddUser.Entity.Id);
    }
}