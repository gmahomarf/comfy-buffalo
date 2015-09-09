using System;
using System.Collections.Generic;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserAbilitiesAdded
    {
        public UserAbilitiesAdded(Guid userId, IEnumerable<Guid> abilitiesGuids)
        {
            UserId = userId;
            AbilitiesGuids = abilitiesGuids;
        }

        public Guid UserId { get; protected set; }
        public IEnumerable<Guid> AbilitiesGuids { get; protected set; }
    }
}