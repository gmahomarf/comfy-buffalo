using System.Threading.Tasks;
using BlingBag;
using Ironhide.Api.Infrastructure;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Api.Modules
{
    public class PasswordResetEmailSender : IBlingHandler<PasswordResetTokenCreated>
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IEmailSender _emailSender;
        readonly IBaseUrlProvider _baseUrlProvider;

        public PasswordResetEmailSender(IReadOnlyRepository readOnlyRepository, IEmailSender emailSender, IBaseUrlProvider baseUrlProvider)
        {
            _readOnlyRepository = readOnlyRepository;
            _emailSender = emailSender;
            _baseUrlProvider = baseUrlProvider;
        }

        public async Task Handle(PasswordResetTokenCreated @event)
        {
            var user = await _readOnlyRepository.GetById<UserEmailLogin>(@event.UserId);
            _emailSender.Send(user.Email, new PasswordResetEmail(_baseUrlProvider.GetBaseUrl(), @event.TokenId));
        }
    }
}