using System;
using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Data.NHibernate.Testing;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using NHibernate;

namespace Ironhide.Data.Specs.WriteableRepositorySpecs
{
    public class when_creating_a_multiple_users
    {
        static IWriteableRepository _writeableRepository;
        static IEnumerable<UserEmailLogin> _result;
        static ISession _session;
        static List<UserEmailLogin> _users;

        Establish context =
            () =>
                {
                    _session = InMemorySession.New(new MappingScheme("Test"));
                    _writeableRepository = new WriteableRepository(_session);

                    _users = new List<UserEmailLogin>
                                 {
                                     new UserEmailLogin("test1", "test1@test.com", new EncryptedPassword("password")),
                                     new UserEmailLogin("test2-match", "test2@test.com", new EncryptedPassword("password")),
                                     new UserEmailLogin("test3", "test2@test.com", new EncryptedPassword("password"))
                                 };
                };

        Because of =
            () => _result = (_writeableRepository.CreateAll(_users).Await()).AsTask.Result;

        It should_all_be_retrievable =
            () => _result.ToList().ForEach(x => _session.Get<UserEmailLogin>(x.Id).Name.ShouldEqual(x.Name));

        It should_all_return_the_created_user_with_an_id =
            () => _result.ToList().ForEach(x => x.Id.ShouldNotEqual(Guid.Empty));
    }
}