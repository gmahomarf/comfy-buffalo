namespace Ironhide.Api.Modules.UserAccounts.Google
{
    public class Result
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string gender { get; set; }
        public string objectType { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
        public Name name { get; set; }
        public string url { get; set; }
        public Image image { get; set; }
        public bool isPlusUser { get; set; }
        public int circledByCount { get; set; }
        public bool verified { get; set; }
    }
}