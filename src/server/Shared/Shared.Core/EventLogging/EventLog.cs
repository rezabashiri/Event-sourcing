using Shared.Core.Contracts;
using Shared.Core.Domain;

namespace Shared.Core.EventLogging;

public record EventLog : Event, IEntity<Guid>
{
    public EventLog(Event theEvent, string data, (string oldValues, string newValues) changes, string email, Guid userId)
    {
        Id = Guid.NewGuid();
        AggregateId = theEvent.AggregateId;
        MessageType = theEvent.MessageType;
        Data = data;
        Email = email;
        OldValues = changes.oldValues;
        NewValues = changes.newValues;
        UserId = userId;
        EventDescription = theEvent.EventDescription;
    }

    protected EventLog()
    {
    }

    public string Data { get; private set; }

    public string OldValues { get; private set; }

    public string NewValues { get; private set; }

    public string Email { get; private set; }

    public Guid UserId { get; private set; }

    public Guid Id { get; set; }
}