using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironhide.Users.Domain.Services
{
    public class JwtSymmetricKeyProvider:IKeyProvider
    {
        private byte[] getBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public byte[] GetKey()
        {
            return getBytes("ThisIsAnImportantStringAndIHaveNoIdeaIfThisIsVerySecureOrNot!");
        }
    }
}
