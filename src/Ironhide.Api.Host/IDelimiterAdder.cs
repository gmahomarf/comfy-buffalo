using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public interface IDelimiterAdder
    {
        IEnumerable<string> AddDelimiters(IEnumerable<string> words);
    }
}