using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class PasswordResetTokenCreated
    {
        public Guid TokenId { get; private set; }
        public Guid UserId { get; private set; }

        public PasswordResetTokenCreated(Guid tokenId, Guid userId)
        {
            TokenId = tokenId;
            UserId = userId;
        }
    }
}