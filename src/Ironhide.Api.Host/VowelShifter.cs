using System;
using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public class VowelShifter : IVowelShifter
    {
        public IEnumerable<string> ShiftRight(IEnumerable<string> words, int numberToShift)
        {
            var vowels = new List<char> { 'a', 'e', 'i', 'o', 'u', 'y', 'A', 'E', 'I', 'O', 'U', 'Y' };

            foreach (var word in words)
            {
                var newWord = word;
                for (int i = 0; i < word.Length; i++)
                {
                    var ch = newWord[i];
                    if (vowels.Contains(ch))
                    {
                        if (i == word.Length - 1)
                        {
                            newWord = ch + newWord.Remove(i, 1);
                        }
                        else
                        {
                            newWord = newWord.Remove(i, 1);
                            newWord = newWord.Insert(i + 1, ch.ToString());
                            i++;
                        }
                    }
                }
                yield return newWord;
            }
        }
    }
}