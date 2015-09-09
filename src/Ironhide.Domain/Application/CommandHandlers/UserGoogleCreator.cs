using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserGoogleCreator : IEventedCommandHandler<IUserSession, CreateGoogleLoginUser>
    {
        readonly IWriteableRepository _writeableRepository;

        public UserGoogleCreator(IWriteableRepository writeableRepository)
        {
            _writeableRepository = writeableRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, CreateGoogleLoginUser command)
        {
            var userCreated = await _writeableRepository.Create(new UserGoogleLogin(command.DisplayName, command.Email, command.Id, command.GivenName, command.FamilyName, command.ImageUrl, command.Url));
            NotifyObservers(new UserGoogleCreated(userCreated.Id, command.Email, command.DisplayName, command.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}