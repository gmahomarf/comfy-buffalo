using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserGoogleCreated : UserCreated
    {
        public string GoogleId { get; set; }

        public UserGoogleCreated(Guid id, string email, string displayName, string googleId) : base(id,email,displayName)
        {
            this.GoogleId = googleId;
        }
    }
}