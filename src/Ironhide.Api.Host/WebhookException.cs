using System;

namespace Ironhide.Api.Host
{
    public class WebhookException : Exception
    {
        public WebhookException()
            : base("Webhook url could not be reached.")
        {            
        }
    }
}