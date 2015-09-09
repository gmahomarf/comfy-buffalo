using System;
using Nancy;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class BadRequestExceptionRepackager : IExceptionRepackager
    {
        #region IExceptionRepackager Members

        public ErrorResponse Repackage(Exception exception, NancyContext context, string contentType)
        {
            return new ErrorResponse(exception.Message, HttpStatusCode.BadRequest, contentType);
        }

        public bool CanHandle(Exception ex, string contentType)
        {
            Type type = ex.GetType();
            return type.Name.Contains("NotValid");
        }

        #endregion
    }
}