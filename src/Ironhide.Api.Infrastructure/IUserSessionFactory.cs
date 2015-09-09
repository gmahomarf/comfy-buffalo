using AcklenAvenue.Commands;
using Nancy.Security;

namespace Ironhide.Api.Infrastructure
{
    public interface IUserSessionFactory
    {
        IUserSession Create(IUserIdentity currentUser);
    }
}