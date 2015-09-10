using FluentAssertions;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_trying_the_process_many_times
    {
        static PostValueResponse _postValueResponse;

        Establish context = () =>
                            {
                                for (int i = 0; i < 30; i++)
                                {
                                    _postValueResponse = CandidateApiTester.RunOnce();
                                }
                            };

        It should_not_fail = () => _postValueResponse.Status.Should().Be("Winner");
    }
}