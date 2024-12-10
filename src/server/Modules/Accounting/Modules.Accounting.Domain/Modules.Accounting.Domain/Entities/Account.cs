using Modules.Accounting.Domain.DomainEvents;
using Modules.Accounting.Domain.Rules;
using Shared.Core.Domain;

namespace Modules.Accounting.Domain.Entities;

public class Account : BaseEntity
{
    /// <summary>
    ///     For ef core
    /// </summary>
    private Account()
        : this(Guid.Empty, string.Empty)
    {
    }

    private Account(Guid userId, string name)
    {
        UserId = userId;
        Name = name;
    }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }

    public DateTime LastSyncedTimespan { get; private set; }
    public static Account Create(Guid userId, string name, decimal initialDeposit)
    {
        var account = new Account(userId, name);
        account.ValidateMinimumAccountBalance(initialDeposit);
        account.Deposit(amount: initialDeposit);
        return account;
    }

    public Deposit Deposit(decimal amount)
    {
        this.ValidateMaximumDeposit(amount);
        decimal newBalance = Balance + amount;
        var depositEvent = new Deposit(amount, Id);
        SetBalance(newBalance, depositEvent.Timestamp);
        AddDomainEvent(depositEvent);
        return depositEvent;
    }

    public void Withdraw(decimal amount)
    {
        this.ValidateMaximumWithdrawInSingleTransaction(amount);
        decimal newBalance = Balance - amount;
        this.ValidateMinimumAccountBalance(newBalance);
        var depositEvent = new WithDraw(amount, Id);
        SetBalance(newBalance, depositEvent.Timestamp);
        AddDomainEvent(depositEvent);
    }

    public void SetBalance(decimal balance, DateTime lastTransactionEventDate)
    {
        Balance = balance;
        LastSyncedTimespan = lastTransactionEventDate;
    }
}