namespace Ironhide.Users.Domain.Entities
{
    public class UserGoogleLogin : User
    {
        public virtual string GoogleId { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string URL { get; protected set; }
        public virtual string ImageUrl { get; protected set; }

        protected UserGoogleLogin()
        {
        }

        public UserGoogleLogin(string name, string email, string googleId, string firstName, string lastName, string imageUrl, string url): base(name, email)
        {
            GoogleId = googleId;
            FirstName = firstName;
            LastName = lastName;
            ImageUrl = imageUrl;
            URL = url;

        }
    }
}