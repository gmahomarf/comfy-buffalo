using System;

namespace Ironhide.Api.Host
{
    public class CandidateSuccess
    {
        public string EmailAddress { get; private set; }
        public string WebhookUrl { get; private set; }
        public DateTime Time { get; private set; }

        public CandidateSuccess(string emailAddress, string webhookUrl, DateTime time)
        {
            EmailAddress = emailAddress;
            WebhookUrl = webhookUrl;
            Time = time;
        }
    }
}