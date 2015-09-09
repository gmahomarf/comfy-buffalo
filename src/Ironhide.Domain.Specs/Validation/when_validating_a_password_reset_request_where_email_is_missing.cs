using System;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Services;
using Ironhide.Users.Domain.Validators;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Domain.Specs.Validation
{
    public class when_validating_a_password_reset_request_where_email_is_missing
    {
        static ICommandValidator<IUserSession, CreatePasswordResetToken> _validator;
        static Exception _exception;
        static IReadOnlyRepository _readOnlyRepsitory;

        Establish context =
            () =>
            {
                _readOnlyRepsitory = Mock.Of<IReadOnlyRepository>();
                _validator = new PasswordResetValidator(_readOnlyRepsitory);
            };

        Because of =
            () => _exception = Catch.Exception(() => _validator.Validate(null, new CreatePasswordResetToken("")).Await());

        It should_return_a_validation_failure_for_missing_email_address =
            () =>
                _exception.As<CommandValidationException>().ValidationFailures
                    .ShouldContain(x => x.Property == "Email" &&
                                        x.FailureType == ValidationFailureType.Missing);
    }
}