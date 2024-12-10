using Modules.Accounting.Domain.Entities;
using Shared.Core.Wrapper;

namespace Modules.Accounting.Application.Accounts.Services;

public interface ITransactionService
{
    Task<IResult<Account>> CalculateBalance(Account account);
    Task<IResult<Account>> FindAccountAndUpdateBalance(Guid accountId);
}