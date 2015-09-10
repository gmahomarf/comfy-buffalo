using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Ironhide.Api.Host
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        static readonly Action<Response> CorsResponse =
            x =>
            {
                x.WithHeader("Access" +
                             "-Control-Allow-Methods",
                    "GET, POST, PUT, DELETE, OPTIONS");
                x.WithHeader("Access-Control-Allow-Headers",
                    "Content-Type, Accept");
                x.WithHeader("Access-Control-Max-Age", "1728000");
                x.WithHeader("Access-Control-Allow-Origin", "*");
            };

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            StaticConfiguration.DisableErrorTraces = false;
            //RestExceptionRepackager.Configure(x => x.WithResponse(CorsResponse)).Register(pipelines);
            pipelines.AfterRequest.AddItemToEndOfPipeline(x => CorsResponse(x.Response));
            base.RequestStartup(container, pipelines, context);
        }
    }
}