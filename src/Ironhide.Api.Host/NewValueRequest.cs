namespace Ironhide.Api.Host
{
    public class NewValueRequest
    {
        public string EncodedValue { get; set; }
        public string EmailAddress { get; set; }
        public string WebhookUrl { get; set; }
    }
}