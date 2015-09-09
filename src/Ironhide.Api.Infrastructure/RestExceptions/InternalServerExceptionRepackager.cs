using System;
using Nancy;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class InternalServerExceptionRepackager : IExceptionRepackager
    {
        #region IExceptionRepackager Members

        public ErrorResponse Repackage(Exception exception, NancyContext context, string contentType)
        {
            string message = AddException(exception);

            string formattedMessage =
                string.Format("The {0} request to '{1}' resulted in an unhandled exception!\r\n\r\n{2}",
                              context.Request.Method, context.Request.Url, message);

            return new ErrorResponse(formattedMessage, HttpStatusCode.InternalServerError, contentType);
        }

        public bool CanHandle(Exception exception, string contentType)
        {
            return false;
        }

        #endregion

        static string AddException(Exception ex)
        {
            string original = string.Format("{0}: {1}\r\n{2}\r\n\r\n",
                                            ex.GetType().Name,
                                            ex.Message,
                                            ex.StackTrace);

            if (ex.InnerException != null)
                original += AddException(ex.InnerException);

            return original;
        }
    }
}