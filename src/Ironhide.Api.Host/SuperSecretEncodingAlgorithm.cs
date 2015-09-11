using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host
{
    public class SuperSecretEncodingAlgorithm : IEncodingAlgorithm
    {
        readonly ICapsAlternator _capsAlternator;
        readonly IDelimiterAdder _delimiterAdder;
        readonly IWordSplitter _wordSplitter;
        readonly IVowelEncoder _vowelEncoder;
        readonly IVowelShifter _vowelShifter;

        public SuperSecretEncodingAlgorithm(IVowelEncoder vowelEncoder, IVowelShifter vowelShifter, ICapsAlternator capsAlternator, IDelimiterAdder delimiterAdder, IWordSplitter wordSplitter)
        {
            _vowelEncoder = vowelEncoder;
            _vowelShifter = vowelShifter;
            _capsAlternator = capsAlternator;
            _delimiterAdder = delimiterAdder;
            _wordSplitter = wordSplitter;
        }

        public string Encode(Int64 startingNumber, string[] words)
        {
            IEnumerable<string> newListWithSplitWords = _wordSplitter.SplitWords(words);
            IOrderedEnumerable<string> alphebeticalOrder = newListWithSplitWords.OrderBy(x => x);
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