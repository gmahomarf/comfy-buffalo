using System;
using System.Collections.Generic;
using Ironhide.Api.Host;
using Ironhide.Api.Host.Algorithms;
using RestSharp;

namespace Ironhide.Api.Specs.Integration
{
    public static class CandidateApiTester
    {
        public static PostValueResponse RunOnce(string webhookUrl)
        {
            string url = "http://localhost:6656";
            //string url = "http://internal-devchallenge-2-dev.apphb.com";
            var client = new RestClient(url);
            Guid guid = Guid.NewGuid();
            var restRequest = new RestRequest("/values/" + guid);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");
            IRestResponse<Values> getValuesResponse = client.Get<Values>(restRequest);
            List<string> words = getValuesResponse.Data.Words;
            Int64 startingFibonacciNumber = getValuesResponse.Data.StartingFibonacciNumber;
            AlgorithmName algorithmName = getValuesResponse.Data.Algorithm;
            
            var encoder = GetEncodingAlgarithm(algorithmName, startingFibonacciNumber);

            string encode = encoder.Encode(words.ToArray());
            string base64EncodedString = new Base64StringEncoder().Encode(encode);
            return PostEncodedValue(guid, base64EncodedString, client, algorithmName, webhookUrl);
        }

        static IEncodingAlgorithm GetEncodingAlgarithm(AlgorithmName algorithmName, long startingFibonacciNumber)
        {
            IEncodingAlgorithm encoder;
            switch (algorithmName)
            {
                case AlgorithmName.Thor:
                    encoder =
                        new ThorAlgorithm(startingFibonacciNumber, new VowelEncoder(new FibonacciGenerator()),
                            new ConsonantCapsAlternator(), new WordSplitter(new StaticDictionary()));
                    break;
                case AlgorithmName.CaptainAmerica:
                    encoder =
                        new CaptainAmericaAlgorithm(startingFibonacciNumber, new VowelEncoder(new FibonacciGenerator()),
                            new VowelShifter(), new AsciiValueDelimiterAdder());
                    break;

                case AlgorithmName.IronMan:
                    encoder =
                        new IronManAlgorithm(new VowelShifter(),new AsciiValueDelimiterAdder());
                    break;

                case AlgorithmName.TheIncredibleHulk:
                    encoder =
                        new TheIncredibleHulkAlgorithm(new VowelShifter());
                    break;
                default:
                    throw new Exception("No matching algoriithm.");
            }
            return encoder;
        }

        static PostValueResponse PostEncodedValue(Guid guid, string encode, RestClient client,
            AlgorithmName algorithmName, string webhookUrl)
        {
            var request = new RestRequest(string.Format("/values/{0}/{1}", guid, algorithmName))
                          {
                              RequestFormat =
                                  DataFormat.Json
                          };
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(new PostValueRequest
                            {
                                EncodedValue = encode,
                                EmailAddress = "byron@acklenavenue.com",
                                WebhookUrl = webhookUrl,
                                RepoUrl = "http://github.com",
                                Name = "Byron Sommardahl"
                            });
            IRestResponse<PostValueResponse> restResponse = client.Post<PostValueResponse>(request);
            return restResponse.Data;
        }
    }
}