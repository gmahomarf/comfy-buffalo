using System;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class RoleNotFound:Exception
    {
        public RoleNotFound(string rol)
            : base(string.Format("The Rol {0} was not configured", rol))
        {
            
        }
    }
}