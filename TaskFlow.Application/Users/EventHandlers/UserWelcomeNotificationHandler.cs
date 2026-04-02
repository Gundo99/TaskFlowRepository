using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.EventHandlers
{
    public class UserWelcomeNotificationHandler : IDomainEventHandler<UserRegisteredEvent>
    {
        public Task Handle(UserRegisteredEvent domainEvent)
        {
            Console.WriteLine($"[WELCOME] Sending welcome message to user {domainEvent.UserId}");

            return Task.CompletedTask;
        }
    }
}
