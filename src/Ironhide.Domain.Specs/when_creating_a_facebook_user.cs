using System;
using AcklenAvenue.Commands;
using FizzWare.NBuilder;
using Ironhide.Users.Domain.Application.CommandHandlers;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Domain.Specs
{
    public class when_creating_a_facebook_user
    {
        static CreateFacebookLoginUser _command;
        static IWriteableRepository _writeableRepository;
        static IEventedCommandHandler<IUserSession, CreateFacebookLoginUser> _handler;
        static UserFacebookCreated _expectedEvent;
        static object _eventRaised;
        static UserFacebookLogin _userCreated;

        Establish context =
            () =>
            {

                _command = Builder<CreateFacebookLoginUser>.CreateNew().Build();

                _writeableRepository = Mock.Of<IWriteableRepository>();
                _userCreated = Builder<UserFacebookLogin>.CreateNew()
                  .With(user => user.Email, _command.email)
                  .With(user => user.Name, _command.name)
                .With(user => user.FacebookId, _command.id)
                .With(user => user.FirstName, _command.firstName)
                .With(user => user.ImageUrl, _command.imageUrl)
                .With(user => user.LastName, _command.lastName)
                .With(user => user.URL, _command.link)
                .With(user => user.Id, Guid.NewGuid())
                  .Build();


                _handler = new UserFacebookCreator(_writeableRepository);

                Mock.Get(_writeableRepository)
                    .Setup(repository => repository.Create(Moq.It.IsAny<UserFacebookLogin>()))
                    .ReturnsAsync(_userCreated);

                _expectedEvent = new UserFacebookCreated(_userCreated.Id,_command.email, _command.name, _command.id);
                _handler.NotifyObservers += x => _eventRaised = x;

            };

        Because of =
            () => _handler.Handle(Mock.Of<IUserSession>(), _command);

        It should_create_facebook_user =
            () => Mock.Get(_writeableRepository).Verify(
                repository => 
                    repository.Create(Moq.It.Is<UserFacebookLogin>(
                        z => z.FirstName == _command.firstName &&
                             z.LastName == _command.lastName &&
                             z.FacebookId == _command.id &&
                             z.ImageUrl == _command.imageUrl &&
                             z.URL == _command.link &&
                             z.Name == _command.name)
                        ));

        It should_throw_the_expected_event =
           () => _eventRaised.ShouldBeLike(_expectedEvent);
    }
}
