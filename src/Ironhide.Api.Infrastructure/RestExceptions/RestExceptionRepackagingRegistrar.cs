using System;
using System.Linq;
using Nancy;
using Nancy.Bootstrapper;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class RestExceptionRepackagingRegistrar
    {
        readonly RestExceptionConfiguration _config;

        public RestExceptionRepackagingRegistrar(RestExceptionConfiguration config)
        {
            _config = config;
        }

        public void Register(IPipelines pipelines)
        {
            pipelines.OnError.AddItemToStartOfPipeline(ConvertToHttpResponse);
        }

        Response ConvertToHttpResponse(NancyContext ctx, Exception err)
        {
            string contentType = ctx.Request.Headers.ContentType;

            IExceptionRepackager handler =
                _config.ErrorHandlers.FirstOrDefault(x => x.CanHandle(err, contentType));

            Response response = handler == null
                                    ? _config.DefaultExceptionRepackager.Repackage(err, ctx, contentType)
                                    : handler.Repackage(err, ctx, contentType);

            _config.ResponseAction(response);

            return response;
        }
    }
}