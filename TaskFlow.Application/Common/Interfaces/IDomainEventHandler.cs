using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Common;

namespace TaskFlow.Application.Common.Interfaces
{
    public interface IDomainEventHandler<TEvent>
        where TEvent : DomainEvent
    {
        Task Handle(TEvent domainEvent);
    }
}
