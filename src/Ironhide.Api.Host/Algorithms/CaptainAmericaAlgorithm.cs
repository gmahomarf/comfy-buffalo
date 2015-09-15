using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host.Algorithms
{
    public class CaptainAmericaAlgorithm : IEncodingAlgorithm
    {
        readonly double _startingNumber;
        readonly IVowelEncoder _vowelEncoder;
        readonly IVowelShifter _vowelShifter;
        readonly IDelimiterAdder _delimiterAdder;
        
        public CaptainAmericaAlgorithm(double startingNumber, IVowelEncoder vowelEncoder, IVowelShifter vowelShifter, IDelimiterAdder delimiterAdder)
        {
            _startingNumber = startingNumber;
            _vowelEncoder = vowelEncoder;
            _vowelShifter = vowelShifter;
            _delimiterAdder = delimiterAdder;
        }

        public string Encode(string[] words)
        {
            IEnumerable<string> listWithVowelsShifted = _vowelShifter.ShiftRight(words, 1);
            var reverseAlphabeticalOrder = listWithVowelsShifted.OrderByDescending(x => x);
            IEnumerable<string> vowelsEncoded = _vowelEncoder.Encode(_startingNumber, reverseAlphabeticalOrder);
            IEnumerable<string> withDelimiters = _delimiterAdder.AddDelimiters(vowelsEncoded).ToList();
            string encoded = string.Join("", withDelimiters);
            return encoded;            
        }
    }
}