using System;
using AcklenAvenue.Data.NHibernate;
using Autofac;
using FluentNHibernate.Cfg.Db;
using Ironhide.Data;
using NHibernate;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class ConfigureDatabaseWiring : IBootstrapperTask<ContainerBuilder>
    {
        #region IBootstrapperTask<ContainerBuilder> Members

        public Action<ContainerBuilder> Task
        {
            get
            {
                MsSqlConfiguration databaseConfiguration = MsSqlConfiguration.MsSql2008.ShowSql().
                    ConnectionString(x => x.Is(ConnectionStrings.Get().ConnectionString)).Dialect<MsSqlAzureDialect>();

                return container =>
                       {
                           container.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As
                               <ISession>()
                               .InstancePerLifetimeScope();

                               container.Register(c =>
                                                  new SessionFactoryBuilder(new MappingScheme(), databaseConfiguration, new EntityInterceptor())
                                                      .Build())
                                   .SingleInstance()
                                   .As<ISessionFactory>();
                           };
            }
        }

        #endregion
    }
}