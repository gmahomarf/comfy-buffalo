using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class DisablingUser : IEventedCommandHandler<IUserSession, DisableUser>
    {
        public IWriteableRepository writeableRepository { get; private set; }
        public IReadOnlyRepository readOnlyRepository { get; private set; }

        public DisablingUser(IWriteableRepository writeableRepository, IReadOnlyRepository readOnlyRepository)
        {
            this.writeableRepository = writeableRepository;
            this.readOnlyRepository = readOnlyRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, DisableUser command)
        {
            var user = await readOnlyRepository.GetById<User>(command.id);
            user.DisableUser();

            await writeableRepository.Update(user);

            NotifyObservers(new UserDisabled(user.Id));


        }

        public event DomainEvent NotifyObservers;
    }
}