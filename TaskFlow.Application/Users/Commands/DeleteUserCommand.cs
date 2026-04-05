
using MediatR;
using TaskFlow.Application.Tasks;

namespace TaskFlow.Application.Users.Commands
{
    public record DeleteUserCommand(Guid UserId) : IRequest<TaskResponse>;
}
