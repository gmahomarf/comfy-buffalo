using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Autofac;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Api.Infrastructure.RestExceptions;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Security;
using Newtonsoft.Json;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        static readonly Action<Response> CorsResponse = x =>
        {
            x.WithHeader("Access-Control-Allow-Methods",
                         "GET, POST, PUT, DELETE, OPTIONS");
            x.WithHeader("Access-Control-Allow-Headers",
                         "Content-Type, Accept");
            x.WithHeader("Access-Control-Max-Age", "1728000");
            x.WithHeader("Access-Control-Allow-Origin", "*");
        };

        readonly List<IBootstrapperTask<ContainerBuilder>> _tasks;

        public Bootstrapper()
        {
            _tasks = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof (IBootstrapperTask<ContainerBuilder>).IsAssignableFrom(x))
                .Select(Activator.CreateInstance)
                .Cast<IBootstrapperTask<ContainerBuilder>>()
                .ToList();
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            var builder = new ContainerBuilder();
            _tasks.ForEach(task => task.Task.Invoke(builder));
            builder.Update(existingContainer.ComponentRegistry);
            base.ConfigureApplicationContainer(existingContainer);
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            StaticConfiguration.DisableErrorTraces = false;

            var configuration =
                new StatelessAuthenticationConfiguration(
                    ctx =>
                    {
                        string token = GetTokenFromRequest(ctx);

                        bool hasToken = !string.IsNullOrEmpty(token);

                        if (hasToken)
                        {
                            var apiUserMapper = container.Resolve<IApiUserMapper<string>>();
                            if (!string.IsNullOrEmpty(token))
                            {
                                try
                                {
                                    IUserIdentity userFromAccessToken =
                                        apiUserMapper.GetUserFromAccessToken(token);
                                    return userFromAccessToken;
                                }
                                catch (TokenDoesNotExistException)
                                {
                                }
                            }
                        }

                        return new LoggedInUserIdentity(new List<Claim>());
                    });

            StatelessAuthentication.Enable(pipelines, configuration);

            RestExceptionRepackager.Configure(x => x.WithResponse(CorsResponse)).Register(pipelines);

            pipelines.AfterRequest.AddItemToEndOfPipeline(x => CorsResponse(x.Response));

            base.RequestStartup(container, pipelines, context);
        }

        static string GetTokenFromRequest(NancyContext ctx)
        {
            var token = (string)ctx.Request.Query.token;
            if (token == null)
            {
                token = (string)ctx.Request.Form.token;
            }
            if (token == null)
            {
                const string headerName = "Authorization";
                bool hasAuthHeader = ctx.Request.Headers.Keys.Contains(headerName);
                if (hasAuthHeader)
                {
                    string authHeader =
                        ctx.Request.Headers[headerName].FirstOrDefault();
                    if (authHeader != null)
                        token = authHeader.Replace("Bearer ", "");
                }
            }
            if (token == null)
            {
                var stream = new StreamReader(ctx.Request.Body);
                string body = stream.ReadToEnd();
                ctx.Request.Body.Seek(0, SeekOrigin.Begin);
                try
                {
                    var bodyWithToken = JsonConvert.DeserializeObject<BodyWithToken>(body);
                    if (bodyWithToken != null && bodyWithToken.Token != Guid.Empty)
                        token = bodyWithToken.Token.ToString();
                }
                catch
                {
                }
            }
            return token;
        }

        public class BodyWithToken
        {
            public Guid Token { get; set; }
        }
    }
}