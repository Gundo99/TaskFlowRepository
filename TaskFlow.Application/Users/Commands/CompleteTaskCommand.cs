using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Tasks;

namespace TaskFlow.Application.Users.Commands
{
    public record CompleteTaskCommand(Guid taskId) : IRequest<TaskResponse>;
}
