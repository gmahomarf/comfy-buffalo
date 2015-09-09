using System.Collections.Generic;

namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminUsersListResponse 
    {
        public List<AdminUserResponse> AdminUsers { get; set; }

        public AdminUsersListResponse(List<AdminUserResponse> adminUsers)
        {
            AdminUsers = adminUsers;
        }
    }
}