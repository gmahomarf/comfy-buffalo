using System.Linq;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserEmailCreator : IEventedCommandHandler<IUserSession, CreateEmailLoginUser>
    {
        readonly IWriteableRepository _writeableRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;

        public UserEmailCreator(IWriteableRepository writeableRepository, IReadOnlyRepository readOnlyRepository)
        {
            _writeableRepository = writeableRepository;
            _readOnlyRepository = readOnlyRepository;
        }

        #region ICommandHandler Members

        public async Task Handle(IUserSession userIssuingCommand, CreateEmailLoginUser command)
        {
            var userCreated = new UserEmailLogin(command.Name, command.Email, command.EncryptedPassword,
                command.PhoneNumber);

            foreach (var ability in command.abilities)
            {
                UserAbility userAbility = await _readOnlyRepository.GetById<UserAbility>(ability.Id);
                userCreated.AddAbility(userAbility);
            }

            var basicRole = await _readOnlyRepository.FirstOrDefault<Role>(x => x.Description == "Basic");
            userCreated.AddRol(basicRole);
            var userSaved = await _writeableRepository.Create(userCreated);

            NotifyObservers(new UserEmailCreated(userSaved.Id, command.Email, command.Name, command.PhoneNumber));
        }

        public event DomainEvent NotifyObservers;

        #endregion
    }
}