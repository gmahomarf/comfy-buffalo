using System;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_getting_the_next_fibonacci_number_after_34
    {
        static readonly FibonacciGenerator FibonacciGenerator = new FibonacciGenerator();
        static double _result;

        Because of =
            () => _result = FibonacciGenerator.GetNext(34);

        It should_return_55 =
            () => Convert.ToInt32(_result).Should().Be(55);
    }
}