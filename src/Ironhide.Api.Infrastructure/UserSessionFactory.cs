using System;
using System.Linq;
using AcklenAvenue.Commands;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Users.Domain;
using Nancy.Security;

namespace Ironhide.Api.Infrastructure
{
    public class UserSessionFactory : IUserSessionFactory
    {
        public IUserSession Create(IUserIdentity currentUser)
        {
            LoggedInUserIdentity loggedInUserIdentity = (LoggedInUserIdentity) currentUser;
            if (HasRole(loggedInUserIdentity, "Administrator"))
            {
                Guid userId = GetUserId(loggedInUserIdentity);
                return new AdminUserSession(userId);
            }
            else if (HasRole(loggedInUserIdentity, "Basic"))
            {
                Guid userId = GetUserId(loggedInUserIdentity);
                return new BasicUserSession(userId);
            }
            return new VisitorSession();
        }

        private static bool HasRole(LoggedInUserIdentity loggedInUserIdentity, string administrator)
        {
            return loggedInUserIdentity.Claims.Any(x => x.Equals(administrator));
        }

        private static Guid GetUserId(LoggedInUserIdentity loggedInUserIdentity)
        {
            return Guid.Parse(loggedInUserIdentity.JwTokenClaims.First(x => x.Type.Equals("userguid")).Value);
        }
    }
}