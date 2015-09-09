using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Data.NHibernate.Testing;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using NHibernate;

namespace Ironhide.Data.Specs.ReadOnlyRepositorySpecs
{
    public class when_getting_a_user_by_id
    {
        static ReadOnlyRepository _readOnlyRepository;
        static UserEmailLogin _result;
        static UserEmailLogin _userToFind;

        Establish context =
            () =>
                {
                    ISession session = InMemorySession.New(new MappingScheme("Test"));

                    var users = new List<UserEmailLogin>
                                    {
                                        new UserEmailLogin("test1", "test1@test.com", new EncryptedPassword("password")),
                                        new UserEmailLogin("test2-match", "test2@test.com", new EncryptedPassword("password")),
                                        new UserEmailLogin("test3", "test2@test.com", new EncryptedPassword("password"))
                                    };

                    _userToFind = users.First(x => x.Name.Contains("match"));

                    users.ForEach(x => session.Save(x));

                    _readOnlyRepository = new ReadOnlyRepository(session);
                };

        Because of =
            () => _result = _readOnlyRepository.GetById<UserEmailLogin>(_userToFind.Id).Await();

        It should_return_matching_user =
            () => _result.ShouldBeLike(_userToFind);
    }
}