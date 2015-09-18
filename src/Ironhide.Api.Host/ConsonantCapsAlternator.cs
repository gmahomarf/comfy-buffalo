using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host
{
    public class ConsonantCapsAlternator : ICapsAlternator
    {
        public IEnumerable<string> Alternate(IEnumerable<string> words)
        {
            const string consonants = "bcdfghjklmnpqrstvwxzBCDFGHJKLMNPQRSTVWXZ";
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
                    if (!consonants.Contains(value))
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