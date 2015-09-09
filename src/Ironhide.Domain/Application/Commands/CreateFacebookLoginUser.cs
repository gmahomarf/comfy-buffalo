namespace Ironhide.Users.Domain.Application.Commands
{
    public class CreateFacebookLoginUser
    {
        public string id { get; protected set; }
        public string email { get;protected set; }
        public string firstName { get; protected set; }
        public string lastName { get; protected set; }
        public string link { get; protected set; }
        public string name { get; protected set; }
        public string imageUrl { get; protected set; }

        protected CreateFacebookLoginUser()
        {
        }

        public CreateFacebookLoginUser(string id, string email, string firstName, string lastName, string link, string name, string imageUrl)
        {
            this.id = id;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.link = link;
            this.name = name;
            this.imageUrl = imageUrl;
        }
    }
}