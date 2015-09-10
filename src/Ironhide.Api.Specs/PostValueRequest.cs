namespace Ironhide.Api.Specs
{
    public class PostValueRequest
    {
        public string EncodedValue { get; set; }
        public string EmailAddress { get; set; }
        public string WebhookUrl { get; set; }
    }
}