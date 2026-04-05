using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Tasks;

namespace TaskFlow.Domain.Users.Commands
{
    public record RegisterUserCommand(string Name, string Email) : IRequest<TaskResponse>;
}
