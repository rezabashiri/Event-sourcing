using Modules.Accounting.Domain.Rules;
using Shared.Core.Exceptions;

namespace Modules.Accounting.Domain.Exceptions;

public class WithdrawMoreThanAllowanceException(Guid accountId, decimal amount, decimal maximum)
    : CustomException("Invalid withdraw", [$"Withdraw amount {amount} for account {accountId} is more than the {RulesConstants.MaximumTransactionPercentage} percentage allowance."]);