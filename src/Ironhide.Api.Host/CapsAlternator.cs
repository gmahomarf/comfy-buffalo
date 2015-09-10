using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host
{
    public class CapsAlternator : ICapsAlternator
    {
        public IEnumerable<string> Alternate(IEnumerable<string> words)
        {
            List<string> theWords = words.ToList();
            string firstLetterOfFirstWord = theWords.First().First().ToString();
            bool caps = firstLetterOfFirstWord.ToUpper() == firstLetterOfFirstWord;
            foreach (string word in theWords)
            {
                string newWord = "";
                foreach (char ch in word)
                {
                    string value = ch.ToString();
                    int num;
                    if (Int32.TryParse(value, out num))
                    {
                        newWord += value;
                    }
                    else
                    {
                        string letter = caps ? value.ToUpper() : value.ToLower();
                        newWord += letter;
                        caps = !caps;
                    }
                }
                yield return newWord;
            }
        }
    }
}