using System.Collections.Generic;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;
using RestSharp;

namespace Ironhide.Api.Specs
{
    public class when_trying_the_process_once
    {
        Establish context = () =>
                            {
                                var client = new RestClient("http://localhost:6656");
                                var restRequest = new RestRequest("/values");
                                restRequest.AddHeader("Accept", "application/json");
                                var restResponse = client.Get<Values>(restRequest);
                                var words = restResponse.Data.Words;

                                var l = 1;
                            };

        public class Values
        {
            public string[] Words { get; set; }
        }

        It should_not_fail = () => { };
    }

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