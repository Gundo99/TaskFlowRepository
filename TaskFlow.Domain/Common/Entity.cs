using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;
        [NotMapped]
        public readonly List<DomainEvent> DomainEventsList = new List<DomainEvent>();
        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
