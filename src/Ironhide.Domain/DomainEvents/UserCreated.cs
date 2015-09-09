using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserCreated
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }

        public UserCreated(Guid id, string email, string name)
        {
            Id = id;
            Email = email;
            Name = name;
        }

        protected UserCreated()
        {
            
        }
    }
}