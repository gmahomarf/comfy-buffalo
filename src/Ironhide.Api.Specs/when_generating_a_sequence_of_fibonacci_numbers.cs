using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_generating_a_sequence_of_fibonacci_numbers
    {
        static IEnumerable<double> _result;

        Because of =
            () => _result = new FibonacciGenerator().Generate(10);

        It should_return_the_correct_sequence =
            () => _result.Select(x=> (int)x)
                .ToArray().Should().Equal(new int[] {1, 1, 2, 3, 5, 8, 13, 21, 34, 55});
    }
}