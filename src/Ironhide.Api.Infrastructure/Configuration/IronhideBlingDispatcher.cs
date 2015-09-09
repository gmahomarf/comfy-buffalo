using System.Collections.Generic;
using BlingBag;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class IronhideBlingDispatcher : ImmediateBlingDispatcher
    {
        public IronhideBlingDispatcher(IEnumerable<IBlingHandler> handlers, IBlingLogger logger) : base(handlers, logger)
        {
        }
    }
}