using System.Threading;
using RestSharp;

namespace SlackClient
{
    public class SlackLogger : ISlackLogger
    {
        readonly RestClient _restClient;

        public SlackLogger()
        {
            _restClient =
                new RestClient("https://acklen.slack.com/services/hooks/incoming-webhook?token=E4kWKWySUvdHjodYm8Pf9oUZ");
        }

        public SlackLogResult Log(SlackPost post)
        {
            var restRequest = new RestRequest {RequestFormat = DataFormat.Json};
            restRequest.AddBody(post);
            IRestResponse restResponse = _restClient.Post(restRequest);
            return new SlackLogResult(restResponse.StatusCode, restResponse.Content);
        }
    }
}