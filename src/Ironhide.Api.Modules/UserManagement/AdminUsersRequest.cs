namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminUsersRequest
    {
        public int PageNumber { get; set; }
        public string Field { get; set; }
        public int PageSize { get; set; }
    }
}