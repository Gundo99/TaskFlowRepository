using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Users.Commands
{
    public record SetUserPasswordCommand(Guid UserId, string Password) : IRequest<Unit>;
}
