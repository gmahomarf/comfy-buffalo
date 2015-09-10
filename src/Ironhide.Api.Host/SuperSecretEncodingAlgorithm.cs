using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host
{
    public class SuperSecretEncodingAlgorithm : IEncodingAlgorithm
    {
        readonly ICapsAlternator _capsAlternator;
        readonly IDelimiterAdder _delimiterAdder;
        readonly IVowelEncoder _vowelEncoder;
        readonly IVowelShifter _vowelShifter;

        public SuperSecretEncodingAlgorithm(IVowelEncoder vowelEncoder, IVowelShifter vowelShifter,
            ICapsAlternator capsAlternator, IDelimiterAdder delimiterAdder)
        {
            _vowelEncoder = vowelEncoder;
            _vowelShifter = vowelShifter;
            _capsAlternator = capsAlternator;
            _delimiterAdder = delimiterAdder;
        }

        public string Encode(double startingNumber, string[] words)
        {
            List<string> alphebeticalOrder = words.OrderBy(x => x).ToList();
            IEnumerable<string> listWithVowelsShifted = _vowelShifter.ShiftRight(alphebeticalOrder, 1);
            IEnumerable<string> vowelsEncoded = _vowelEncoder.Encode(startingNumber, listWithVowelsShifted);
            IEnumerable<string> consonantsCapsAlternated = _capsAlternator.Alternate(vowelsEncoded);
            IOrderedEnumerable<string> reverseAlphabeticalOrder = consonantsCapsAlternated.OrderByDescending(x => x);
            IEnumerable<string> withDelimiters = _delimiterAdder.AddDelimiters(reverseAlphabeticalOrder).ToList();
            string encoded = string.Join("", withDelimiters);
            return encoded;
        }
    }
}