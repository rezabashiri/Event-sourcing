using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Domain.Abstractions;
using Modules.Accounting.Domain.Entities;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Commands;

public class CreateAccountHandler(IAccountingDbContext accountingDbContext, IStringLocalizer<CreateAccountCommand> localizer) : IRequestHandler<CreateAccountCommand, IResult>
{
    public async Task<IResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await accountingDbContext.Users.Include(x => x.Accounts)
            .FirstOrDefaultAsync(x => request.UserId.Equals(x.Id), cancellationToken);
        if (user is null)
        {
            return Result.Fail(localizer["User does not exists!"]);
        }

        user.Accounts.Add(Account.Create(user.Id, request.Name, request.InitialDeposit));

        await accountingDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}