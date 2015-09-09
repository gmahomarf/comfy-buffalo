using System;

namespace Ironhide.Users.Domain.Entities
{
   

    public class UserAbility : Entity
    {

        public virtual string Description { get; protected set; }

        protected UserAbility()
        {
            
        }

        public UserAbility(string description)
        {
            this.Description = description;
            Id = Guid.NewGuid();

        }


    }
}
