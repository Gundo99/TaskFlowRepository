using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Tasks.Commands
{
    public class UpdateTaskCommand
    {
        public Guid TaskId { get; }
        public string Title { get; }
        public string? Description { get; }

        public UpdateTaskCommand(Guid taskId, string title, string? description)
        {
            TaskId = taskId;
            Title = title;
            Description = description;
        }
    }
}
