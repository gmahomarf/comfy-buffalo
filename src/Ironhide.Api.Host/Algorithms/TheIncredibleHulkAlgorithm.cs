using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host.Algorithms
{
    public class TheIncredibleHulkAlgorithm : IEncodingAlgorithm
    {
        readonly IVowelShifter _vowelShifter;
        
        public TheIncredibleHulkAlgorithm(IVowelShifter vowelShifter)
        {
            _vowelShifter = vowelShifter;
        }

        public string Encode(string[] words)
        {
            IEnumerable<string> listWithVowelsShifted = _vowelShifter.ShiftRight(words, 1);
            IOrderedEnumerable<string> reverseAlphabeticalOrder = listWithVowelsShifted.OrderByDescending(x => x);
            var delimited = string.Join("*", reverseAlphabeticalOrder);
            return delimited;
        }
    }
}