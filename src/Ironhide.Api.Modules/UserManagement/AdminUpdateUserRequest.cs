using System;

namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminUpdateUserRequest
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Profile { get; set; }

        public string Name { get; set; }
    }
}