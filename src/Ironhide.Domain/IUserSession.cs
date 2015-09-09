using System;

namespace Starscream.Domain
{
    public interface IUserSession
    {
        Guid Id { get; }
    }
}