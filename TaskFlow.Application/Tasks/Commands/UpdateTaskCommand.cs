using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Tasks.Commands
{
   public record UpdateTaskCommand(Guid taskId, string title, string? description) : IRequest<TaskResponse>;
}
