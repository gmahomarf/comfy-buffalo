using System;
using AcklenAvenue.Commands;

namespace Ironhide.Users.Domain
{
    public class AdminUserSession : IUserSession
    {
        public Guid UserId { get; private set; }


        public AdminUserSession(Guid userId)
        {
            UserId = userId;
        }

        public Guid Id { get; private set; }
    }
}