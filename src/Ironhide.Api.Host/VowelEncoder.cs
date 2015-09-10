using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public class VowelEncoder : IVowelEncoder
    {
        readonly INumberSeriesGenerator _numberGenerator;

        public VowelEncoder(INumberSeriesGenerator numberGenerator)
        {
            _numberGenerator = numberGenerator;
        }

        public IEnumerable<string> Encode(double startingNumberDelimiter, IEnumerable<string> wordsWithVowels)
        {
            double nextNum = startingNumberDelimiter;
            foreach (string word in wordsWithVowels)
            {
                string newWord = "";
                foreach (char ch in word)
                {
                    var vowels = new List<char> {'a', 'e', 'i', 'o', 'u', 'y', 'A', 'E', 'I', 'O', 'U', 'Y'};

                    if (vowels.Contains(ch))
                    {
                        newWord += nextNum.ToString();
                        nextNum = _numberGenerator.GetNext(nextNum);                        
                    }
                    else
                    {
                        newWord += ch;
                    }
                }
                yield return newWord;                
            }
        }
    }
}