using System;

namespace Starscream.Domain
{
    public interface ITimeProvider
    {
        DateTime Now();
    }
}