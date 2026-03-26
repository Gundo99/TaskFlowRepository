using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Tasks.Events
{
    public class TaskCompletedEvent : DomainEvent
    {
        public Guid TaskId { get; }
        public TaskCompletedEvent(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
