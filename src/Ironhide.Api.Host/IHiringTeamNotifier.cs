using System.Threading.Tasks;

namespace Ironhide.Api.Host
{
    public interface IHiringTeamNotifier
    {
        Task Notify(string emailAddress, string name, string repoUrl, string code);
    }
}