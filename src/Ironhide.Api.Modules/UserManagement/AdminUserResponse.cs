using System;

namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
    }
}