using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Machine.Specifications;

namespace Ironhide.Api.Specs.Integration
{
    [Ignore("Integration test")]
    public class when_trying_the_process_many_times
    {
        static PostValueResponse _postValueResponse;
        static readonly List<PostValueResponse> Responses = new List<PostValueResponse>();

        Establish context = () =>
                            {
                                for (int i = 0; i < 30; i++)
                                {
                                    _postValueResponse = CandidateApiTester.RunOnce("http://requestb.in/1hb5qvz1");
                                    Responses.Add(_postValueResponse);
                                }
                            };

        It should_have_no_failures = () => Responses.Any(x => x.Status == "CrashAndBurn").Should().BeFalse();
    }
}