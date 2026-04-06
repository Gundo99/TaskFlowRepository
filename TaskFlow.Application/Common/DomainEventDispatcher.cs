using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Common;

namespace TaskFlow.Application.Common
{
    public class DomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Dispatch(IEnumerable<DomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var handlerType = typeof(IDomainEventHandler<>)
                    .MakeGenericType(domainEvent.GetType());

                var handlers = _serviceProvider.GetService(handlerType);

                foreach (var handler in (IEnumerable<object>)handlers)
                {
                    var method = handlerType.GetMethod("Handle");

                    await (Task)method.Invoke(handler, new object[] { domainEvent });
                }
            }
        }
    }
}
