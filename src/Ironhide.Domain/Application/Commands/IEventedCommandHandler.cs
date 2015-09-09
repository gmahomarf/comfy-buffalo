using AcklenAvenue.Commands;

namespace Ironhide.Users.Domain.Application.Commands
{
    public interface IEventedCommandHandler<in TUserSession, in TCommand> : ICommandHandler<TUserSession, TCommand>
    {
        event DomainEvent NotifyObservers;
    }
}