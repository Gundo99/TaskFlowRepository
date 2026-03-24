using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Tasks.Commands
{
    public class CreateTaskCommand
    {
        public Guid UserId { get; }
        public string Title { get; }
        public string? Description { get; }
        public CreateTaskCommand(Guid userId, string title, string? description)
        {
            UserId = userId;
            Title = title;
            Description = description;
        }
    }
}
