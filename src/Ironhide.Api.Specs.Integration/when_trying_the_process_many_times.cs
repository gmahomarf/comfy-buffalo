using System.Collections.Generic;
using FluentAssertions;
using Machine.Specifications;

namespace Ironhide.Api.Specs.Integration
{
    //[Ignore("Integration test")]
    public class when_trying_the_process_many_times
    {
        static PostValueResponse _postValueResponse;

        static List<string> _failures = new List<string>();
        
        Establish context = () =>
                            {
                                for (int i = 0; i < 30; i++)
                                {
                                    _postValueResponse = CandidateApiTester.RunOnce("http://requestb.in/1hb5qvz1");
                                    if (_postValueResponse.Status == "CrashAndBurn")
                                    {
                                        _failures.Add(_postValueResponse.Message);
                                    }
                                }
                            };

        It should_end_with_a_win = () => _postValueResponse.Status.Should().Be("Winner");

        It should_have_no_failures = () => _failures.Should().BeEmpty();
    }    
}