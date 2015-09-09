using System.Collections.Generic;

namespace Ironhide.Api.Infrastructure.Authentication.Roles
{
    public class UsersRoles
    {
        public string Name { get; set; }
        public IEnumerable<Feature> Features { get; set; }  

    }
}