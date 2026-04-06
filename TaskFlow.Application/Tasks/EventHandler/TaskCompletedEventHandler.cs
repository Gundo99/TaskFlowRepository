using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Tasks.Events;

namespace TaskFlow.Application.Tasks.EventHandler
{
    public class TaskCompletedEventHandler : IDomainEventHandler<TaskCompletedEvent>    
    {
        public Task Handle(TaskCompletedEvent domainEvent)
        {
            Console.WriteLine($"Task completed : {domainEvent}");

            return Task.CompletedTask;
        }
    }
}
