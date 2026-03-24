using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Users.Commands
{
    public class CompleteTaskCommand
    {
        public Guid TaskId { get; }
        public CompleteTaskCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
