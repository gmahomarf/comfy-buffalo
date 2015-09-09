using System;

namespace Starscream.Domain
{
    public class GuidTokenGenerator : ITokenGenerator<Guid>
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}