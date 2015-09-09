using System;

namespace Ironhide.Api.Infrastructure
{
    public interface IBootstrapperTask<in T>
    {
        Action<T> Task { get; }
    }
}
