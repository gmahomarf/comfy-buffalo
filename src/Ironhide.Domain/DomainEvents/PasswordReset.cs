using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class PasswordReset
    {
        public Guid UserId { get; private set; }

        public PasswordReset(Guid userId)
        {
            UserId = userId;
        }
    }
}