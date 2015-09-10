using System.Collections.Generic;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_adding_delimiters_to_words
    {
        static IEnumerable<string> _result;
        static readonly AsciiValueDelimiterAdder AsciiValueDelimiterAdder = new AsciiValueDelimiterAdder();

        Because of =
            () => _result = AsciiValueDelimiterAdder.AddDelimiters(new[] {"byron", "sommardahl"});

        It should_return_a_list_of_words_with_the_expected_delimiters =
            () =>
            {
                _result.Should().Equal(new[] {"byron", ((int) 's').ToString(), "sommardahl", ((int) 'b').ToString()});
            };
    }
}