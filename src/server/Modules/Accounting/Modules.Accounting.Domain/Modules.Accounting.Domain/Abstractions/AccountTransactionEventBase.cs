using System.Text.Json.Serialization;
using Shared.Core.Domain;

namespace Modules.Accounting.Domain.Abstractions;

public record AccountTransactionEventBase : DomainEvent
{
    [JsonConstructor]
    public AccountTransactionEventBase(decimal amount, Guid aggregateId, string messageType)
        : base(aggregateId)
    {
        MessageType = messageType;
        Amount = amount;
    }
    /// <summary>
    ///     To create base event
    /// </summary>
    /// <param name="amount">transaction amount</param>
    /// <param name="aggregateId">Use account id as aggregateid</param>
    public AccountTransactionEventBase(decimal amount, Guid aggregateId)
        : base(aggregateId)
    {
        Amount = amount;
    }
    public decimal Amount { get; set; }
}