using System;

namespace Ironhide.Users.Domain.Services
{
    public interface ITokenExpirationProvider
    {
        DateTime GetExpiration(DateTime now);
    }
}