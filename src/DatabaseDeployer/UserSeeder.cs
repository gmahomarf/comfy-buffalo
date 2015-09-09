using System;
using DomainDrivenDatabaseDeployer;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using NHibernate;

namespace DatabaseDeployer
{
    public class UserSeeder : IDataSeeder
    {
        readonly ISession _session;

        public UserSeeder(ISession session)
        {
            _session = session;
        }

        #region IDataSeeder Members

        public void Seed()
        {
            var encryptor = new HashPasswordEncryptor();

            var admiRole = new Role(Guid.NewGuid(), "Administrator");
            var basicRole = new Role(Guid.NewGuid(), "Basic");
            var basicUser = new UserEmailLogin("Test User", "test@test.com", encryptor.Encrypt("password"), "615-555-1212");
            basicUser.AddRol(basicRole);
            var administratorUser = new UserEmailLogin("Admin User", "admin@test.com", encryptor.Encrypt("password"),
                "123");
            administratorUser.AddRol(admiRole);
            administratorUser.AddRol(basicRole);

            var userAbilityDev = new UserAbility("Developer");
            _session.Save(userAbilityDev);
            basicUser.AddAbility(userAbilityDev);

            var userAbilityAdmin = new UserAbility("Administrator");
            _session.Save(userAbilityAdmin);
            administratorUser.AddAbility(userAbilityAdmin);

            _session.Save(admiRole);
            _session.Save(basicRole);


            _session.Save(basicUser);
            _session.Save(administratorUser);


        }

        #endregion
    }
}