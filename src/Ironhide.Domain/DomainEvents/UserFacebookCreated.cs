using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserFacebookCreated:UserCreated
    {

        public string FacebookId { get; protected set; }

        public UserFacebookCreated(Guid id, string email, string name, string facebookId) : base(id,email,name )
        {
    
            this.FacebookId = facebookId;
        }
    }
}