using Shared.Core.Exceptions;

namespace Modules.Accounting.Domain.Exceptions;

public class DepositMoreThanMaximumException(Guid accountId, decimal amount, double maximum)
    : CustomException("Invalid deposit", [$"Deposit amount {amount} for account {accountId} is more than the maximum amount of {maximum}"]);