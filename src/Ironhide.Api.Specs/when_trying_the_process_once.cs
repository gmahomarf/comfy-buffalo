using FluentAssertions;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_trying_the_process_once
    {
        static PostValueResponse _postValueResponse;

        Establish context = () => _postValueResponse = CandidateApiTester.RunOnce();

        It should_not_fail = () => _postValueResponse.Status.Should().Be("Success");
    }
}