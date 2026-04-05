using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Tasks;
using TaskFlow.Application.Users.Queries.GetUsers;

namespace TaskFlow.Application.Users.Commands
{
    public record UpdateUserEmailCommand(Guid UserId, string Email) : IRequest<UserResponse>;
}
