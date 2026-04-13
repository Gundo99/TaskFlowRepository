using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Users.Queries.GetUsers;

namespace TaskFlow.Application.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<UserResponse>;
}
