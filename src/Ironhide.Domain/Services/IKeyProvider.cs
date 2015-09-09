using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironhide.Users.Domain.Services
{
    public interface IKeyProvider
    {
        byte[] GetKey();
    }
}
