using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Email Email { get; set; }

        public DateTime CreatedAt { get; set; }

        private User() { }
        public User(string name, Email email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            CreatedAt = DateTime.UtcNow;
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
