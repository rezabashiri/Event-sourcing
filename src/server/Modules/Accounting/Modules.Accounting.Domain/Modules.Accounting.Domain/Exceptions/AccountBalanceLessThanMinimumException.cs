using Shared.Core.Exceptions;

namespace Modules.Accounting.Domain.Exceptions;

public class AccountBalanceLessThanMinimumException : CustomException
{
    public AccountBalanceLessThanMinimumException(Guid accountId, decimal amount, decimal minimum)
        : base($"Account balance will be {amount} for account {accountId} which is less than the minimum amount of {minimum}")
    {
    }
}