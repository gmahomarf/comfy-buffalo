using System.Net;

namespace SlackClient
{
    public class SlackLogResult
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string Content { get; private set; }

        public SlackLogResult(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}