namespace Modules.Accounting.Domain.Rules;

public static class RulesConstants
{
    public const decimal MinimumAccountAmount = 100;
    public const double MaximumDepositAmount = 10000d;
    public const ushort MaximumTransactionPercentage = 90;
}