using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.EventHandlers
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
    {
        public Task Handle(UserRegisteredEvent domainEvent)
        {
            Console.WriteLine($"[LOG]User registered: {domainEvent.UserId}");

            return Task.CompletedTask;
        }
    }
}
