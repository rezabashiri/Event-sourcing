using Modules.Accounting.Domain.Entities;
using Modules.Accounting.Domain.Exceptions;

namespace Modules.Accounting.Domain.Rules;

internal static class TransactionRules
{
    public static void ValidateMinimumAccountBalance(this Account account, decimal amount)
    {
        if (amount < RulesConstants.MinimumAccountAmount)
        {
            throw new AccountBalanceLessThanMinimumException(account.Id, amount, RulesConstants.MinimumAccountAmount);
        }
    }

    public static void ValidateMaximumDeposit(this Account account, decimal amount)
    {
        if (amount > (decimal)RulesConstants.MaximumDepositAmount)
        {
            throw new DepositMoreThanMaximumException(account.Id, amount, RulesConstants.MaximumDepositAmount);
        }
    }
    public static void ValidateMaximumWithdrawInSingleTransaction(this Account account, decimal amount)
    {
        decimal transactionMaximumAllowedAmount = account.Balance * RulesConstants.MaximumTransactionPercentage / 100;
        if (amount > transactionMaximumAllowedAmount)
        {
            throw new WithdrawMoreThanAllowanceException(account.Id, amount, Math.Round(transactionMaximumAllowedAmount, 2));
        }
    }
}