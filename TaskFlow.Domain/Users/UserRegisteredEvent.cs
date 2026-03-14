using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Users
{
    public class UserRegisteredEvent : DomainEvent
    {
        public Guid UserId { get; }
        public UserRegisteredEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
