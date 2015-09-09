using System;
using System.Security.Cryptography;
using System.Text;

namespace Starscream.Domain
{
    public class HashPasswordEncryptor : IPasswordEncryptor
    {
        #region IPasswordEncryptor Members

        public EncryptedPassword Encrypt(string clearTextPassword)
        {
            byte[] encryptedPassword = HashPassword(clearTextPassword);
            string base64String = Convert.ToBase64String(encryptedPassword);
            return new EncryptedPassword(base64String);
        }

        #endregion

        public static byte[] HashPassword(string password)
        {
            var provider = new SHA1CryptoServiceProvider();
            var encoding = new UnicodeEncoding();
            return provider.ComputeHash(encoding.GetBytes(password));
        }
    }
}