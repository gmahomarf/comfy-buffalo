using System;
using AcklenAvenue.Commands;

namespace Ironhide.Users.Domain
{
    public class BasicUserSession : IUserSession
    {
        public Guid UserId { get; private set; }

        public BasicUserSession(Guid userId)
        {
            UserId = userId;
        }

        public Guid Id { get; private set; }
    }
}