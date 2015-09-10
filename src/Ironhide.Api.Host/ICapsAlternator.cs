using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public interface ICapsAlternator
    {
        IEnumerable<string> Alternate(IEnumerable<string> words);
    }
}