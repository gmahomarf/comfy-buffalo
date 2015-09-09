using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class EnablingUser : IEventedCommandHandler<IUserSession, EnableUser>
    {
        readonly IWriteableRepository _writeableRepository;
        readonly IReadOnlyRepository _readOnlyRepository;

        public EnablingUser(IWriteableRepository writeableRepository, IReadOnlyRepository readOnlyRepository)
        {
            _writeableRepository = writeableRepository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, EnableUser command)
        {
            var user = await _readOnlyRepository.GetById<User>(command.id);

            user.EnableUser();

            await _writeableRepository.Update(user);

            NotifyObservers(new UserEnabled(user.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}