using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AcklenAvenue.Commands;
using AcklenAvenue.Email;
using Autofac;
using AutoMapper;
using BlingBag;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Api.Infrastructure.Authentication.Roles;
using Ironhide.Data;
using Ironhide.EmailClients.DotNet;
using Ironhide.TemplateEngines.Razor;
using Ironhide.Users.Domain;
using log4net;
using Newtonsoft.Json;
using NHibernate;

namespace Ironhide.Api.Infrastructure.Configuration
{    
    public class ConfigureCommonDependencies : IBootstrapperTask<ContainerBuilder>
    {
        #region IBootstrapperTask<ContainerBuilder> Members

        public Action<ContainerBuilder> Task
        {
            get
            {
                return container =>
                       {
                           AutoRegisterDataAndDomain(container);
                           container.RegisterInstance(Mapper.Engine).As<IMappingEngine>();
                           container.RegisterType<BaseUrlProvider>().As<IBaseUrlProvider>();
                           container.RegisterType<ApiUserMapper>().As<IApiUserMapper<string>>();
                           container.RegisterInstance(LogManager.GetLogger("Logger")).As<ILog>();
                           container.RegisterType<UserSessionFactory>().As<IUserSessionFactory>();
                           log4net.Config.XmlConfigurator.Configure();
                          
                           ConfigureCommandAndEventHandlers(container);
                           AutoRegisterEmailTemplates(container);

                           AutoRegisterAllDomainEvents(container);
                           AutoRegisterAllCommandHandlers(container);
                           RegisterUsersFeutures(container);
                       };
            }
        }

        void RegisterUsersFeutures(ContainerBuilder container)
        {


            var bytes = Properties.Resources.RolesFeatures;
            var reader = new StreamReader(new MemoryStream(bytes), Encoding.Default);


            var usersRoles = new JsonSerializer().Deserialize<IEnumerable<UsersRoles>>(new JsonTextReader(reader));
            

            container.RegisterType<MenuProvider>().As<IMenuProvider>().WithParameter("usersRoles",usersRoles);

        }

        void AutoRegisterAllCommandHandlers(ContainerBuilder container)
        {
            container.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(x => x.GetInterfaces().Any(i => i.Name.StartsWith("ICommandHandler")))
                .AsImplementedInterfaces();
        }

        void AutoRegisterAllDomainEvents(ContainerBuilder container)
        {
            container.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(x => x.GetInterfaces().Any(i => i.Name.StartsWith("IBlingHandler")))
                .AsImplementedInterfaces();
        }

        #endregion

        static void ConfigureCommandAndEventHandlers(ContainerBuilder container)
        {
            container.RegisterType<IronhideBlingDispatcher>().As<IBlingDispatcher>();
            
            container.RegisterType<ImmediateCommandDispatcher>().Named<ICommandDispatcher>("CommandDispatcher");
            container.RegisterDecorator<ICommandDispatcher>(
                (c, inner) => new UnitOfWorkCommandDispatcher(inner, c.Resolve<ISession>()), "CommandDispatcher");

            container.RegisterType<CommandDispatcherLogger>().As<ICommandDispatcherLogger>();
        }

        static void AutoRegisterEmailTemplates(ContainerBuilder container)
        {
            container.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(x => typeof (IEmailBodyTemplate).IsAssignableFrom(x) ||
                            typeof (IEmailSubjectTemplate).IsAssignableFrom(x)
                ).AsImplementedInterfaces();
        }

        static void AutoRegisterDataAndDomain(ContainerBuilder container)
        {
            container
                .RegisterAssemblyTypes(new[]
                                       {
                                           typeof (ReadOnlyRepository).Assembly,
                                           typeof (IEntity).Assembly,
                                           typeof (MailGunSmtpClient).Assembly,
                                           typeof (EmailSender).Assembly,
                                           typeof (RazorViewEngine).Assembly                                          
                                       })
                .Where(x => x.GetInterfaces().Any())
                .Where(x => typeof(ICommandHandler).IsAssignableFrom(x)!=true)
                
                .AsImplementedInterfaces();
        }
    }

}