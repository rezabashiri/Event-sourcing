using Modules.Accounting.Domain.Abstractions;

namespace Modules.Accounting.Domain.DomainEvents;

public record WithDraw : AccountTransactionEventBase
{
    public WithDraw(decimal amount, Guid aggregateId)
        : base(amount, aggregateId)
    {
    }
}