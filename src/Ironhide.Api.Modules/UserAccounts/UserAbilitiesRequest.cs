using System;
using System.Collections.Generic;

namespace Ironhide.Api.Modules.UserAccounts
{
    public class UserAbilitiesRequest
    {
        public IEnumerable<UserAbilityRequest> Abilities { get; set; }
        public Guid UserId { get; set; }
    }
}