using FluentAssertions;
using Machine.Specifications;

namespace Ironhide.Api.Specs.Integration
{
    public class when_trying_the_process_once
    {
        static PostValueResponse _postValueResponse;

        Establish context = () => _postValueResponse = CandidateApiTester.RunOnce("http://requestb.in/1hb5qvz1");

        It should_not_fail = () => _postValueResponse.Status.Should().Be("Success");
    }
}