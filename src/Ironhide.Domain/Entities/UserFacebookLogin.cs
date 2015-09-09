namespace Ironhide.Users.Domain.Entities
{
    public class UserFacebookLogin:User
    {
        public UserFacebookLogin(string name, string email, string facebookId, string firstName, string lastName, string imageUrl, string url) : base(name, email)
        {
            FacebookId = facebookId;
            FirstName = firstName;
            LastName = lastName;
            ImageUrl = imageUrl;
            URL = url;
        }

        protected UserFacebookLogin()
        {

        }

        public virtual string FacebookId { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string  LastName { get; protected set; }
        public virtual string URL { get; protected set; }
        public virtual string ImageUrl { get; protected set; }

        


    }
}