namespace Starscream.Domain.ValueObjects
{
    public class EmailAddress
    {

        public string Email { get; protected set; }

        public EmailAddress( string email)
        {
            Email = email;
        }

        protected EmailAddress()
        {
            
        }
    }
}