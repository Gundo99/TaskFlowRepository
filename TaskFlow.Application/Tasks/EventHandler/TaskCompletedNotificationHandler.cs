using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Tasks.Events;

namespace TaskFlow.Application.Tasks.EventHandler
{
    public class TaskCompletedNotificationHandler : IDomainEventHandler<TaskCompletedEvent>
    {
        public Task Handle(TaskCompletedEvent domainEvent)
        {
            Console.WriteLine($"[NOTIFICATION] Task {domainEvent.TaskId} completed. Sending notification...)");

            return Task.CompletedTask;
        }
    }
}
