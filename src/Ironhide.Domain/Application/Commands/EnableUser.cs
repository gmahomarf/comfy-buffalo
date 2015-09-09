using System;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class EnableUser
    {
        public Guid id { get; protected set; }
    

        public EnableUser(Guid id)
        {
            this.id = id;
    
        }

        protected EnableUser()
        {
            
        }
    }
}