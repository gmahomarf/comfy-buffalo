using System.Linq;

namespace Ironhide.Api.Host
{
    public class StaticDictionary : IEnglishDictionary
    {
        readonly string[] words =
        {
            "cats", "rule", "dogs", "drool", "clean", "code", "materials", "needed", "this", "is", "hard",
            "what", "are", "you", "smoking", "shot", "gun", "down", "river", "super", "man"
        };

        public bool IsEnglishWord(string word)
        {
            return words.Select(x => x.ToLower()).Contains(word.Trim().ToLower());
        }
    }
}