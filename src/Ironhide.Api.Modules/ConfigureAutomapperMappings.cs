using System;
using Autofac;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Modules.UserAccounts;
using Ironhide.Api.Modules.UserManagement;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Api.Modules
{
    public class ConfigureAutomapperMappings : IBootstrapperTask<ContainerBuilder>
    {
        #region IBootstrapperTask<ContainerBuilder> Members

        public Action<ContainerBuilder> Task
        {
            get
            {
                return container =>
                    {
                        Mapper.CreateMap<User, AdminUserResponse>();
                        Mapper.CreateMap<UserAbility, UserAbilityRequest>().ReverseMap();
                    };
            }
        }

        #endregion
    }
}