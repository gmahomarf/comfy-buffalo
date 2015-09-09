using System;

namespace Starscream.Domain
{
    public class UserSessionFactory : IUserSessionFactory
    {
        readonly ITimeProvider _timeProvider;
        readonly ITokenExpirationProvider _tokenExpirationProvider;
        readonly ITokenGenerator<Guid> _tokenGenerator;
        readonly IWriteableRepository _writeableRepository;

        public UserSessionFactory(IWriteableRepository writeableRepository,
                                  ITimeProvider timeProvider,
                                  ITokenGenerator<Guid> tokenGenerator,
                                  ITokenExpirationProvider tokenExpirationProvider)
        {
            _writeableRepository = writeableRepository;
            _timeProvider = timeProvider;
            _tokenGenerator = tokenGenerator;
            _tokenExpirationProvider = tokenExpirationProvider;
        }

        #region IUserSessionFactory Members

        public UserSession Create(User executor)
        {
            var userSession = new UserSession
                                  {
                                      Id = _tokenGenerator.Generate(),
                                      User = executor,
                                      Expires = _tokenExpirationProvider.GetExpiration(_timeProvider.Now())
                                  };

            _writeableRepository.Create(userSession);

            return userSession;
        }

        #endregion
    }
}