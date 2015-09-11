using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public interface IWordSplitter
    {
        IEnumerable<string> SplitWords(IEnumerable<string> words);
    }
}