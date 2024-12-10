using FluentAssertions;
using Modules.Accounting.Domain.DomainEvents;
using Modules.Accounting.Domain.Entities;
using Modules.Accounting.Domain.Exceptions;
using Modules.Accounting.Domain.Rules;

namespace Modules.Accounting.UnitTests.Entities;

public class AccountTests
{
    [Fact]
    public void CreateAccount_ShouldCreateAccount_WhenInitialDepositIsValid()
    {
        var userId = Guid.NewGuid();
        string name = "Reza";
        decimal initialDeposit = 500;
        var account = Account.Create(userId, name, initialDeposit);

        account.Should().NotBeNull();
        account.UserId.Should().Be(userId);
        account.Name.Should().Be(name);
        account.DomainEvents.Should().ContainSingle(e => e is Deposit);
    }

    [Fact]
    public void Deposit_ShouldAddDeposit_WhenAmountIsWithinValidRange()
    {
        var userId = Guid.NewGuid();
        var account = Account.Create(userId, "Reza", 500);

        account.Deposit(200);

        account.DomainEvents.Should().HaveCount(2);
        account.DomainEvents.Should().Contain(e => e is Deposit);
    }

    [Fact]
    public void Deposit_ShouldThrowException_WhenAmountIsLessThanMinimum()
    {
        var userId = Guid.NewGuid();

        var act = () => Account.Create(userId, "Reza", 50);

        act.Should().Throw<AccountBalanceLessThanMinimumException>();
    }

    [Fact]
    public void Deposit_ShouldThrowException_WhenAmountIsGreaterThanMaximum()
    {
        var userId = Guid.NewGuid();
        var account = Account.Create(userId, "Reza", 500);
        decimal depositAmount = 20000;

        var act = () => account.Deposit(depositAmount);

        act.Should().Throw<DepositMoreThanMaximumException>();
    }

    [Fact]
    public void Withdraw_ThrowsWithdrawMoreThanAllowanceException_WhenAmountExceedsAllowance()
    {
        var account = Account.Create(Guid.NewGuid(), "Test Account", 1000m);
        decimal excessiveAmount = 1000m * RulesConstants.MaximumTransactionPercentage / 100m;

        Assert.Throws<WithdrawMoreThanAllowanceException>(() => account.Withdraw(excessiveAmount + 1));
    }

    [Fact]
    public void Withdraw_ThrowsAccountBalanceLessThanMinimumException_WhenBalanceIsLessThanMinimumAfterWithdraw()
    {
        var account = Account.Create(Guid.NewGuid(), "Test Account", 101m);

        Assert.Throws<AccountBalanceLessThanMinimumException>(() => account.Withdraw(2));
    }

    [Fact]
    public void Withdraw_AddsDomainEvent_WhenValidWithdrawal()
    {
        var account = Account.Create(Guid.NewGuid(), "Test Account", 1000m);
        decimal withdrawAmount = 50m;

        account.Withdraw(withdrawAmount);

        Assert.Contains(account.DomainEvents, e => e is WithDraw);
    }
}