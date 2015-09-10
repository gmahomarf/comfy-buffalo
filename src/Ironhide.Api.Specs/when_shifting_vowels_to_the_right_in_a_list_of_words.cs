using System.Collections.Generic;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_shifting_vowels_to_the_right_in_a_list_of_words
    {
        static VowelShifter _shifter;
        static IEnumerable<string> _result;

        Establish context =
            () => { _shifter = new VowelShifter(); };

        Because of =
            () => _result = _shifter.ShiftRight(new List<string> {"byron", "sommardahl", "donaldo"}, 1);

        It should_return_a_list_with_the_same_words_but_with_vowels_shifted_to_the_right_by_one =
            () => _result.ShouldBeEquivalentTo(new List<string> { "bryno", "smomradhal", "odnolad" });
    }
}