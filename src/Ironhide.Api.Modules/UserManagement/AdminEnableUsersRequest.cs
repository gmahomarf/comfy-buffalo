using System;

namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminEnableUsersRequest
    {
        public Guid Id { get; set; }
        public bool Enable { get; set; }
    }
}