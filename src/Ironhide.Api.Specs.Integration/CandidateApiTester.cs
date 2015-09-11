using System;
using System.Collections.Generic;
using Ironhide.Api.Host;
using RestSharp;

namespace Ironhide.Api.Specs.Integration
{
    public static class CandidateApiTester
    {
        public static PostValueResponse RunOnce()
        {
            string url = "http://localhost:6656";
            //string url = "http://internal-comfybuffalo-1-dev.apphb.com";
            var client = new RestClient(url);
            Guid guid = Guid.NewGuid();
            var restRequest = new RestRequest("/values/" + guid);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");                        
            IRestResponse<Values> getValuesResponse = client.Get<Values>(restRequest);
            List<string> words = getValuesResponse.Data.Words;
            Int64 startingFibonacciNumber = getValuesResponse.Data.StartingFibonacciNumber;

            var encoder =
                new SuperSecretEncodingAlgorithm(new VowelEncoder(new FibonacciGenerator()),
                    new VowelShifter(), new CapsAlternator(), new AsciiValueDelimiterAdder(), new WordSplitter(new StaticDictionary()));

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
                                WebhookUrl = "http://requestb.in/11qswdy1",
                                RepoUrl= "http://github.com",
                                Name= "Byron Sommardahl"

                            });
            IRestResponse<PostValueResponse> restResponse = client.Post<PostValueResponse>(request);
            return restResponse.Data;
        }
    }
}