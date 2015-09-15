using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host.Algorithms
{
    public class IronManAlgorithm : IEncodingAlgorithm
    {
        readonly IVowelShifter _vowelShifter;
        readonly IDelimiterAdder _delimiterAdder;
        
        public IronManAlgorithm(IVowelShifter vowelShifter, IDelimiterAdder delimiterAdder)
        {
            _vowelShifter = vowelShifter;
            _delimiterAdder = delimiterAdder;
        }

        public string Encode(string[] words)
        {
            IOrderedEnumerable<string> alphebeticalOrder = words.OrderBy(x => x);
            IEnumerable<string> listWithVowelsShifted = _vowelShifter.ShiftRight(alphebeticalOrder, 1);
            IEnumerable<string> withDelimiters = _delimiterAdder.AddDelimiters(listWithVowelsShifted).ToList();
            string encoded = string.Join("", withDelimiters);
            return encoded;           
        }
    }
}