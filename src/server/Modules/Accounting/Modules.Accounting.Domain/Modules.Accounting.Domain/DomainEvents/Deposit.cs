using Modules.Accounting.Domain.Abstractions;

namespace Modules.Accounting.Domain.DomainEvents;

public record Deposit : AccountTransactionEventBase
{
    public Deposit(decimal amount, Guid aggregateId)
        : base(amount, aggregateId)
    {
    }
}