using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Users
{
    public class User : Entity
    {

        public string? Name { get; private set; }
        public Role Role { get; private set; } = Role.User;
        public Email Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public List<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

        private User() { }
        public User(string name, Email email, string passwordHash, Role role = Role.User)
        {
            if (email == null)
                throw new ArgumentException("Email cannot be empty.");

            Id = Guid.NewGuid();
            Email = email;
            CreatedAt = DateTime.UtcNow;
            Name = name;
            PasswordHash = passwordHash;
            Role = role;

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

        public void SetPassword(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
    }
}
