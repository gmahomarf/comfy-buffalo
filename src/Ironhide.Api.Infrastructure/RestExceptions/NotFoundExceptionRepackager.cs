using System;
using Nancy;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class NotFoundExceptionRepackager : IExceptionRepackager
    {
        #region IErrorHandler Members

        public ErrorResponse Repackage(Exception exception, NancyContext context, string contentType)
        {
            return new ErrorResponse(exception.Message, HttpStatusCode.NotFound, contentType);
        }

        public bool CanHandle(Exception ex, string contentType)
        {
            return ex.GetType().Name.Contains("NotFound");
        }

        #endregion
    }
}