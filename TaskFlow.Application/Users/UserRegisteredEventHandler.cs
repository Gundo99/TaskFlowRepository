using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
    {
        public Task Handle(UserRegisteredEvent domainEvent)
        {
            Console.WriteLine($"User registered: {domainEvent.UserId}");

            return Task.CompletedTask;
        }
    }
}
