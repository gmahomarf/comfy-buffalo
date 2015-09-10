using System;
using System.Text;

namespace Ironhide.Api.Host
{
    public class Base64StringEncoder : IStringEncoder
    {
        public string Encode(string unencoded)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(unencoded);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}