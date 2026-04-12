using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = string.Empty;

        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public RefreshToken() { }
        public RefreshToken(Guid userId, string token, DateTime expiryDate)
        {
            UserId = userId;
            Token = token;
            ExpiryDate = expiryDate;
            IsRevoked = false;
        }

        public void Revoke()
        {
            IsRevoked = true;
        }

        public bool IsActive()
        {
            return DateTime.UtcNow >= ExpiryDate;
        }
    }
}
