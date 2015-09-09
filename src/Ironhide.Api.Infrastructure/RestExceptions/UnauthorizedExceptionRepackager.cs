using System;
using Nancy;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class UnauthorizedExceptionRepackager : IExceptionRepackager
    {
        #region IErrorHandler Members

        public ErrorResponse Repackage(Exception exception, NancyContext context, string contentType)
        {
            return new ErrorResponse(exception.Message, HttpStatusCode.Unauthorized, contentType);
        }

        public bool CanHandle(Exception ex, string contentType)
        {
            return ex is UnauthorizedAccessException;
        }

        #endregion
    }
}