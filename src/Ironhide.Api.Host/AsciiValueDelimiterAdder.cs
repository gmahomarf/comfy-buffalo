using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host
{
    public class AsciiValueDelimiterAdder : IDelimiterAdder
    {
        public IEnumerable<string> AddDelimiters(IEnumerable<string> words)
        {
            string[] arr = words.ToArray();
            var list = new List<string>();
            for (int i = 0; i < arr.Length; i++)
            {
                list.Add(arr[i]);
                if (i == 0)
                {
                    var lastWord = arr.Last();
                    list.Add(((int) lastWord[0]).ToString());
                }
                else
                {
                    list.Add(((int) arr[i - 1][0]).ToString());
                }                
            }
            return list.ToArray();
        }
    }
}