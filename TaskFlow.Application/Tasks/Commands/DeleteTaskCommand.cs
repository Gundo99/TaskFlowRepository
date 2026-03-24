using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Tasks.Commands
{
    public class DeleteTaskCommand
    {
        public Guid TaskId { get; }

        public DeleteTaskCommand(Guid taskId) { TaskId = taskId; }
    }
}
