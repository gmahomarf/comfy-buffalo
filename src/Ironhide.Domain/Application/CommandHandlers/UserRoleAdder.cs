using System;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserRoleAdder : IEventedCommandHandler<IUserSession, AddRoleToUser>
    {
        private readonly IWriteableRepository _writeableRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        
        public UserRoleAdder(IWriteableRepository writeableRepository, IReadOnlyRepository readOnlyRepository)
        {
            _writeableRepository = writeableRepository;
            _readOnlyRepository = readOnlyRepository;            
        }

        public async Task Handle(IUserSession userIssuingCommand, AddRoleToUser command)
        {

            var user = await _readOnlyRepository.GetById<User>(command.UserId);
            var role = await _readOnlyRepository.GetById<Role>(command.RolId);

            user.AddRol(role);

            await _writeableRepository.Update(user);
            NotifyObservers(new UserRoleAdded(user.Id, role.Id));


        }

        public event DomainEvent NotifyObservers;
    }
}