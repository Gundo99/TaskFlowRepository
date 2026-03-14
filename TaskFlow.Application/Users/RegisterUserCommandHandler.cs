using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        public Task Handle(RegisterUserCommand command)
        {
            var email = new Email(command.Email);

            var user = new User(command.Name, email);

            Console.WriteLine($"User created: {user.Id}");

            return Task.CompletedTask;
        }
    }
}
