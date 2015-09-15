using System;

namespace Ironhide.Api.Host
{
    public interface IHiringTeamNotifier
    {
        void Notify(string emailAddress, string name, string repoUrl, string code, TimeSpan timeToWinFromFirstSuccess);
    }
}