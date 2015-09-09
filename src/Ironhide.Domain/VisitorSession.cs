using System;
using AcklenAvenue.Commands;

namespace Ironhide.Users.Domain
{
    public class VisitorSession : IUserSession
    {
        #region IUserSession Members

        public Guid Id { get; private set; }

        #endregion
    }
}