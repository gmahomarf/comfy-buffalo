using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserAbilitiesAdder : IEventedCommandHandler<IUserSession, AddAbilitiesToUser>
    {
        readonly IWriteableRepository _writeableRepository;
        readonly IReadOnlyRepository _readOnlyRepository;

        public UserAbilitiesAdder(IWriteableRepository writeableRepository, IReadOnlyRepository readOnlyRepository)
        {
            _writeableRepository = writeableRepository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, AddAbilitiesToUser command)
        {
            var user = await _readOnlyRepository.GetById<User>(command.UserId);
            var abilities = new List<UserAbility>();
            foreach (var abilityId in command.Abilities)
            {
                UserAbility userAbility = await _readOnlyRepository.GetById<UserAbility>(abilityId);
                abilities.Add(userAbility);
                user.AddAbility(userAbility);
            }

            await _writeableRepository.Update(user);

            NotifyObservers(new UserAbilitiesAdded(user.Id, abilities.Select(x=>x.Id)));
        }

        public event DomainEvent NotifyObservers;
    }
}