using System;

namespace Ironhide.Users.Domain.Services
{
    public interface ITimeProvider
    {
        DateTime Now();
    }
}