using System;
using System.Collections.Generic;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;
using RestSharp;

namespace Ironhide.Api.Specs
{
    public static class CandidateApiTester
    {
        public static PostValueResponse RunOnce()
        {
            var client = new RestClient("http://localhost:6656");
            Guid guid = Guid.NewGuid();
            var restRequest = new RestRequest("/values/" + guid);
            restRequest.AddHeader("Accept", "application/json");
            IRestResponse<Values> getValuesResponse = client.Get<Values>(restRequest);
            List<string> words = getValuesResponse.Data.Words;
            double startingFibonacciNumber = getValuesResponse.Data.StartingFibonacciNumber;

            var encoder =
                new SuperSecretEncodingAlgorithm(new VowelEncoder(new FibonacciGenerator()),
                    new VowelShifter(), new CapsAlternator(), new AsciiValueDelimiterAdder());

            string encode = encoder.Encode(startingFibonacciNumber, words.ToArray());
            string s = new Base64StringEncoder().Encode(encode);
            return PostEncodedValue(guid, s, client);
        }

        static PostValueResponse PostEncodedValue(Guid guid, string encode, RestClient client)
        {
            var request = new RestRequest("/values/" + guid) {RequestFormat = DataFormat.Json};
            request.AddHeader("Content-Type", "application/json");            
            request.AddBody(new PostValueRequest
                            {
                                EncodedValue = encode,
                                EmailAddress = "byron@acklenavenue.com",
                                WebhookUrl = "http://requestb.in/11qswdy1"
                            });
            IRestResponse<PostValueResponse> restResponse = client.Post<PostValueResponse>(request);
            return restResponse.Data;
        }
    }

    public class when_trying_the_process_once
    {
        static PostValueResponse _postValueResponse;

        Establish context = () => _postValueResponse = CandidateApiTester.RunOnce();

        It should_not_fail = () => _postValueResponse.Status.Should().Be("Success");
    }

    public class when_trying_the_process_many_times
    {
        static PostValueResponse _postValueResponse;

        Establish context = () =>
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    _postValueResponse = CandidateApiTester.RunOnce();
                                }
                            };

        It should_not_fail = () => _postValueResponse.Status.Should().Be("Winner");
    }

    public class PostValueResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class PostValueRequest
    {
        public string EncodedValue { get; set; }
        public string EmailAddress { get; set; }
        public string WebhookUrl { get; set; }
    }

    public class Values
    {
        public List<string> Words { get; set; }
        public double StartingFibonacciNumber { get; set; }
    }
}