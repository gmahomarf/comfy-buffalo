using Nancy.Security;

namespace Ironhide.Api.Infrastructure.Authentication
{
    public interface IApiUserMapper<in T>
    {
        IUserIdentity GetUserFromAccessToken(T token);
    }
}