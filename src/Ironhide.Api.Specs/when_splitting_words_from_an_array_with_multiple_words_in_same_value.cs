using System.Collections.Generic;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Api.Specs
{
    public class when_splitting_words_from_an_array_with_multiple_words_in_same_value
    {
        static WordSplitter _splitter;
        static IEnumerable<string> _result;

        Establish context =
            () =>
            {
                var englishDictionary = Mock.Of<IEnglishDictionary>();
                _splitter = new WordSplitter(englishDictionary);

                Mock.Get(englishDictionary).Setup(x => x.IsEnglishWord("two")).Returns(true);
                Mock.Get(englishDictionary).Setup(x => x.IsEnglishWord("three")).Returns(true);
                Mock.Get(englishDictionary).Setup(x => x.IsEnglishWord("five")).Returns(true);
                Mock.Get(englishDictionary).Setup(x => x.IsEnglishWord("six")).Returns(true);
            };

        Because of =
            () => _result = _splitter.SplitWords(new List<string> {"one", "twothree", "four", "fivesix"});

        It should_return_a_larger_list_with_words_split_out =
            () => _result.ShouldBeEquivalentTo(new List<string> {"one", "two", "three", "four", "five", "six"});
    }
}