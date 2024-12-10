using MediatR;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Domain.Abstractions;
using Modules.Accounting.Domain.Events;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Commands;

public class DeleteAccountHandler(IAccountingDbContext accountingDbContext, IStringLocalizer<DeleteAccountHandler> localizer) : IRequestHandler<DeleteAccountCommand, IResult>
{
    public async Task<IResult> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await accountingDbContext.Accounts.FindAsync(request.AccountId);

        if (account == null)
        {
            return Result.Fail(localizer["Account not found!"]);
        }

        account.AddDomainEvent(new AccountDeleted(account));
        accountingDbContext.Accounts.Remove(account);

        await accountingDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}