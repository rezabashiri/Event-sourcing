using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using Newtonsoft.Json;

namespace Shared.Core.Domain;

public abstract record Event : Message, INotification
{
    protected Event(string description = null)
    {
        Timestamp = DateTime.Now;
        AggregateId = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(description))
        {
            EventDescription = description;
        }
    }

    [NotMapped]
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public IEnumerable<Type> RelatedEntities { get; protected init; } = new List<Type>();

    public DateTime Timestamp { get; init; }

    public string EventDescription { get; init; }
}