using System;
using System.Collections.Generic;
using System.Linq;
using Ironhide.Users.Domain.Exceptions;
using Nancy;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class CommandValidationExceptionRepackager : IExceptionRepackager
    {
        public ErrorResponse Repackage(Exception exception, NancyContext context, string contentType)
        {
            IEnumerable<string> failures =
                ((CommandValidationException) exception).ValidationFailures.Select(
                    x => string.Format("{0} ({1})", x.Property, x.FailureType));
            string message = string.Format("{0}: {1}", exception.Message, string.Join(", ", failures));
            return new ErrorResponse(message, HttpStatusCode.BadRequest, contentType);
        }

        public bool CanHandle(Exception exception, string contentType)
        {
            return exception is CommandValidationException;
        }
    }
}