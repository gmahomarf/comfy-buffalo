using System;
using AcklenAvenue.Commands;
using AcklenAvenue.Testing.Moq;
using AcklenAvenue.Testing.Moq.ExpectedObjects;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Application.CommandHandlers;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Domain.Specs
{
    public class when_creating_a_password_reset_token
    {
        const string EmailAddress = "email@email.com";
        static IEventedCommandHandler<IUserSession, CreatePasswordResetToken> _handler;
        static IWriteableRepository _writeableRepository;
        static ITimeProvider _timeProvider;
        static DateTime _now;
        static UserEmailLogin _userWithMatchingEmailAddress;
        static object _eventRaised;
        static readonly Guid TokenId = Guid.NewGuid();
        static PasswordResetTokenCreated _expectedEvent;
        static IIdentityGenerator<Guid> _idGenerator;

        Establish context =
            () =>
            {
                var readOnlyRepository = Mock.Of<IReadOnlyRepository>();
                _writeableRepository = Mock.Of<IWriteableRepository>();
                _timeProvider = Mock.Of<ITimeProvider>();
                _idGenerator = Mock.Of<IIdentityGenerator<Guid>>();
                _handler = new PasswordResetTokenCreator(_writeableRepository, readOnlyRepository, _timeProvider,
                    _idGenerator);

                Mock.Get(_idGenerator).Setup(x => x.Generate()).Returns(TokenId);

                _now = DateTime.Now;
                Mock.Get(_timeProvider).Setup(x => x.Now()).Returns(_now);

                _userWithMatchingEmailAddress = new UserEmailLogin("Test User", EmailAddress,
                    new EncryptedPassword("some password"));
                var otherUser = new UserEmailLogin("Other User", "other@email.com", new EncryptedPassword("password"));
                Mock.Get(readOnlyRepository).Setup(x => x.First(ThatHas.AnExpressionFor<UserEmailLogin>()
                    .ThatMatches(_userWithMatchingEmailAddress)
                    .ThatDoesNotMatch(otherUser)
                    .Build()))
                    .ReturnsAsync(_userWithMatchingEmailAddress);

                _handler.NotifyObservers += x => _eventRaised = x;
                _expectedEvent = new PasswordResetTokenCreated(TokenId, _userWithMatchingEmailAddress.Id);
            };

        Because of =
            () => _handler.Handle(new VisitorSession(), new CreatePasswordResetToken(EmailAddress));

        It should_create_the_password_reset_token =
            () => Mock.Get(_writeableRepository)
                .Verify(
                    x =>
                        x.Create(
                            WithExpected.Object(new PasswordResetAuthorization(TokenId, _userWithMatchingEmailAddress.Id,
                                _now), AllowAnonymous.No)));

        It should_raise_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);
    }
}