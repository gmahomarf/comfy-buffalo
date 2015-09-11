namespace Ironhide.Api.Specs.Integration
{
    public class PostValueRequest
    {
        public string EncodedValue { get; set; }
        public string EmailAddress { get; set; }
        public string WebhookUrl { get; set; }
        public string RepoUrl { get; set; }
        public string Name { get; set; }
    }
}