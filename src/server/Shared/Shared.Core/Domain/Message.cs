using Shared.Core.Utilities;

namespace Shared.Core.Domain;

public abstract record Message
{

    protected Message()
    {
        MessageType = GetType().GetGenericTypeName();
    }
    public string MessageType { get; protected init; }

    public Guid AggregateId { get; protected init; }
}