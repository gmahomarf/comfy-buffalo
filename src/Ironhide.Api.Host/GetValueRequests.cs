using System;

namespace Ironhide.Api.Host
{
    public class GetValueRequests
    {
        public Guid Guid { get; private set; }

        public GetValueRequests(Guid guid)
        {
            Guid = guid;
        }
    }
}