using System;
using AcklenAvenue.Data.NHibernate.Testing;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Machine.Specifications;
using NHibernate;

namespace Ironhide.Data.Specs.ReadOnlyRepositorySpecs
{
    public class when_getting_first_user_by_query_with_no_match
    {
        static ReadOnlyRepository _readOnlyRepository;
        static Exception _exception;

        Establish context =
            () =>
                {
                    ISession session = InMemorySession.New(new MappingScheme("Test"));
                    _readOnlyRepository = new ReadOnlyRepository(session);
                };

        Because of =
            () => _exception = Catch.Exception(() => _readOnlyRepository.First<UserEmailLogin>(x => x.Name == "test2-match").Await());

        It should_throw_the_expected_exception =
            () => _exception.ShouldBeOfExactType<ItemNotFoundException<UserEmailLogin>>();
    }
}