using System.Collections.Generic;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_alternating_caps_on_consonants_in_words_in_a_list
    {
        static ConsonantCapsAlternator _capsAlternator;
        static IEnumerable<string> _result;

        Establish context =
            () => { _capsAlternator = new ConsonantCapsAlternator();};

        Because of =
            () => _result = _capsAlternator.Alternate(new List<string> { "also", "byron", "sommardahl", "d5na1d" });

        It should_return_the_same_list_with_consonant_case_alternating_starting_with_the_case_of_the_first_letter =
            () => _result.ShouldBeEquivalentTo(new List<string> { "alSo", "byRon", "SomMarDahL", "d5Na1d" });
    }
}