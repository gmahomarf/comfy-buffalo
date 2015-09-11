using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public class WordSplitter : IWordSplitter  
    {
        readonly IEnglishDictionary _englishDictionary;

        public WordSplitter(IEnglishDictionary englishDictionary)
        {
            _englishDictionary = englishDictionary;
        }

        public IEnumerable<string> SplitWords(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                if (_englishDictionary.IsEnglishWord(word))
                    yield return word;

                var newWord = "";                    
                for (int i = 0; i < word.Length; i++)
                {
                    newWord += word[i];
                    if (_englishDictionary.IsEnglishWord(newWord) || i==word.Length-1)
                    {
                        yield return newWord;
                        newWord = "";
                    }
                }                
            }            
        }
    }
}