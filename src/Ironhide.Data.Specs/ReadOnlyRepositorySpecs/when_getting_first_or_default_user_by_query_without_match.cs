using System.Collections.Generic;
using AcklenAvenue.Data.NHibernate.Testing;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using NHibernate;

namespace Ironhide.Data.Specs.ReadOnlyRepositorySpecs
{
    public class when_getting_first_or_default_user_by_query_without_match
    {
        static ReadOnlyRepository _readOnlyRepository;
        static UserEmailLogin _result;
        
        Establish context =
            () =>
                {
                    ISession session = InMemorySession.New(new MappingScheme("Test"));

                    var users = new List<UserEmailLogin>
                                    {
                                        new UserEmailLogin("test1", "test1@test.com", new EncryptedPassword("password")),
                                        new UserEmailLogin("test2", "test2@test.com", new EncryptedPassword("password")),
                                        new UserEmailLogin("test3", "test2@test.com", new EncryptedPassword("password"))
                                    };

                    users.ForEach(x => session.Save(x));
                    session.Flush();

                    _readOnlyRepository = new ReadOnlyRepository(session);
                };

        Because of =
            () => _result = _readOnlyRepository.FirstOrDefault<UserEmailLogin>(x => x.Name == "test2-match").Await();

        It should_return_matching_user =
            () => ShouldExtensionMethods.ShouldBeNull(_result);
    }
}