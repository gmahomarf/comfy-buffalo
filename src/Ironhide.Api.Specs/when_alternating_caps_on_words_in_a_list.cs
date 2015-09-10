using System.Collections.Generic;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_alternating_caps_on_words_in_a_list
    {
        static CapsAlternator _capsAlternator;
        static IEnumerable<string> _result;

        Establish context =
            () => { _capsAlternator = new CapsAlternator(); };

        Because of =
            () => _result = _capsAlternator.Alternate(new List<string> {"byron", "sommardahl", "d5na1d"});

        It should_return_the_same_list_with_caps_alternating_starting_with_first_letter =
            () => _result.ShouldBeEquivalentTo(new List<string> {"bYrOn", "SoMmArDaHl", "D5nA1d"});
    }
}