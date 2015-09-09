using System;

namespace Ironhide.Users.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}