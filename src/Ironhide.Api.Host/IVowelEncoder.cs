using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public interface IVowelEncoder
    {
        IEnumerable<string> Encode(double startingNumber, IEnumerable<string> wordsWithVowels);
    }
}