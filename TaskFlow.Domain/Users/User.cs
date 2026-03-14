using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Users
{
    public class User : Entity
    {

        public string? Name { get; private set; }

        public Email Email { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private User() { }
        public User(string name, Email email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (email == null)
                throw new ArgumentException("Email cannot be empty.");

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserRegisteredEvent(Id));
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            Name = name;
        }

        public void UpdateEmail( Email email)
        {
            if (email == null)
                throw new ArgumentException("Email cannot be empty.");
            Email = email;
        }
    }
}
