using System;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class AddRoleToUser 
    {
        public Guid UserId { get; protected set; }
        public Guid RolId { get; protected set; }

        protected AddRoleToUser()
        {            
        }

        public AddRoleToUser(Guid userId, Guid rolId)
        {
            UserId = userId;
            RolId = rolId;
        }
    }
}