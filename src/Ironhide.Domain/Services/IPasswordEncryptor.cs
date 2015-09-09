using Ironhide.Users.Domain.ValueObjects;

namespace Ironhide.Users.Domain.Services
{
    public interface IPasswordEncryptor
    {
        EncryptedPassword Encrypt(string clearTextPassword);
    }
}