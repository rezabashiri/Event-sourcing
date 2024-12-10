using System;
using System.Collections.Generic;
using Shared.Core.Contracts;

namespace Shared.Core.Domain
{
    public abstract class BaseEntity : IEntity<Guid>, IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        private List<Event> _domainEvents;

        public IReadOnlyCollection<Event> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(Event domainEvent)
        {
            _domainEvents ??= new List<Event>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(Event domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}