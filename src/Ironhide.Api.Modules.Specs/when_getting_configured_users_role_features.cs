using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Api.Infrastructure.Authentication.Roles;
using Ironhide.Api.Infrastructure.RestExceptions;
using Machine.Specifications;

namespace Ironhide.Api.Modules.Specs
{
    public class when_getting_configured_users_role_features
    {
         private static IEnumerable<UsersRoles> _usersRoles;
         private static string[] _expectedUsersRoles;
        private static string[] _resultRoles;
        private static IMenuProvider _menuProvider;
        private static string adminRolName;
        private static string basicRoleName;
 
        private Establish context =
            () =>
            {
                 adminRolName = "Administrator";
                 basicRoleName = "Basic";
                var feutures1 = Builder<Feature>.CreateListOfSize(1).Build();
                var feutures2 = Builder<Feature>.CreateListOfSize(1).Build();


                _usersRoles = new List<UsersRoles>()
                {
                    new UsersRoles() {Name = adminRolName, Features = feutures1},
                    new UsersRoles() {Name = basicRoleName, Features = feutures2}

                };

                _menuProvider = new MenuProvider(_usersRoles);

                _expectedUsersRoles = new[] {
                    feutures1.FirstOrDefault().Description, 
                    feutures2.FirstOrDefault().Description
                };


            };

        private Because of =
            () =>
            {
                _resultRoles = _menuProvider.getFeatures(new[] {adminRolName, basicRoleName});
            };

         It should_return_all_users_rols =
            () => _resultRoles.ShouldBeLike(_expectedUsersRoles);

         It should_throw_rol_not_configured_exception = () =>
         {
             var exception = Catch.Exception(() => _menuProvider.getFeatures(new[] {"Not configured"}));
             exception.ShouldBeOfExactType<RoleNotFound>();

         };
    }
}
