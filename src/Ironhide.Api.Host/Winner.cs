using System;

namespace Ironhide.Api.Host
{
    public class Winner
    {
        public string EmailAddress { get; private set; }
        public string Name { get; private set; }
        public string RepoUrl { get; private set; }
        public DateTime Time { get; private set; }

        public Winner(string emailAddress, string name, string repoUrl, DateTime time)
        {
            EmailAddress = emailAddress;
            Name = name;
            RepoUrl = repoUrl;
            Time = time;
        }
    }
}