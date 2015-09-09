using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Data.NHibernate.Testing;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using NHibernate;

namespace Ironhide.Data.Specs.ReadOnlyRepositorySpecs
{
    public class when_querying_for_users
    {
        static ReadOnlyRepository _readOnlyRepository;
        static IEnumerable<UserEmailLogin> _result;
        static List<UserEmailLogin> _users;

        Establish context =
            () =>
                {
                    ISession session = InMemorySession.New(new MappingScheme("Test"));

                    _users = new List<UserEmailLogin>
                                 {
                                     new UserEmailLogin("test1", "test1@test.com", new EncryptedPassword("password")),
                                     new UserEmailLogin("test2-match", "test2@test.com", new EncryptedPassword("password")),
                                     new UserEmailLogin("test3-match", "test2@test.com", new EncryptedPassword("password"))
                                 };

                    _users.ForEach(x => session.Save(x));
                    session.Flush();

                    _readOnlyRepository = new ReadOnlyRepository(session);
                };

        Because of =
            () => _result = _readOnlyRepository.Query<UserEmailLogin>(x => x.Name.Contains("match")).Await().AsTask.Result;

        It should_return_the_matching_users =
            () => _result.ShouldBeLike(_users.Where(x => x.Name.Contains("match")));
    }
}