using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host.Algorithms
{
    public class ThorAlgorithm : IEncodingAlgorithm
    {
        readonly long _startingFibonacciNumber;
        readonly IVowelEncoder _vowelEncoder;
        readonly ICapsAlternator _capsAlternator;
        readonly IWordSplitter _wordSplitter;

        public ThorAlgorithm(long startingFibonacciNumber, IVowelEncoder vowelEncoder, ICapsAlternator capsAlternator, IWordSplitter wordSplitter)
        {
            _startingFibonacciNumber = startingFibonacciNumber;
            _vowelEncoder = vowelEncoder;
            _capsAlternator = capsAlternator;
            _wordSplitter = wordSplitter;
        }

        public string Encode(string[] words)
        {
            IEnumerable<string> newListWithSplitWords = _wordSplitter.SplitWords(words);
            IOrderedEnumerable<string> alphebeticalOrder = newListWithSplitWords.OrderBy(x => x);
            IEnumerable<string> consonantsCapsAlternated = _capsAlternator.Alternate(alphebeticalOrder);
            IEnumerable<string> vowelsEncoded = _vowelEncoder.Encode(_startingFibonacciNumber, consonantsCapsAlternated);
            var withDelimiters = string.Join("*", vowelsEncoded);
            string encoded = string.Join("", withDelimiters);
            return encoded;            
        }
    }
}