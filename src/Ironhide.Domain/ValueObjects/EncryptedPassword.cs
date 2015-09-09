namespace Ironhide.Users.Domain.ValueObjects
{
    public class EncryptedPassword
    {
        public string Password { get; set; }

        public EncryptedPassword(string encryptedPassword)
        {
            Password = encryptedPassword;
        }
    }
}