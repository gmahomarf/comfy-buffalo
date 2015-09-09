namespace Ironhide.Users.Domain.Application.Commands
{
    public class CreateGoogleLoginUser
    {
        public string Id { get; protected set; }
        public string Email { get; protected set; }
        public string GivenName { get; protected set; }
        public string FamilyName { get; protected set; }
        public string Url { get; protected set; }
        public string DisplayName { get; protected set; }
        public string ImageUrl { get; protected set; }

        protected CreateGoogleLoginUser()
        {
        }

        public CreateGoogleLoginUser(string id, string email, string givenName, string familyName, string url, string displayName, string imageUrl)
        {
            Id = id;
            Email = email;
            GivenName = givenName;
            FamilyName = familyName;
            Url = url;
            DisplayName = displayName;
            ImageUrl = imageUrl;
        }
    }
}