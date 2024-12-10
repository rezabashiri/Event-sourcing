using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.Accounts.DTOs;
using Modules.Accounting.Application.Accounts.Services;
using Modules.Accounting.Domain.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Commands;

public class WithdrawCommandHandler(IAccountingDbContext accountingDbContext, IMapper mapper, ITransactionService transactionService, IStringLocalizer<WithdrawCommandHandler> localizer) : IRequestHandler<WithdrawCommand, IResult<AccountDTo>>
{
    public async Task<IResult<AccountDTo>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var accountBalanceResult = await transactionService.FindAccountAndUpdateBalance(request.AccountId);

        if (!accountBalanceResult.Succeeded)
        {
            return Result<AccountDTo>.Fail(accountBalanceResult.Messages);
        }

        accountBalanceResult.Data.Withdraw(request.Amount);

        await accountingDbContext.SaveChangesAsync(cancellationToken);

        return Result<AccountDTo>.Success(mapper.Map<AccountDTo>(accountBalanceResult.Data));
    }
}