using System.Threading.Tasks;

namespace SlackClient
{
    public interface ISlackLogger
    {
        Task<SlackLogResult> Log(SlackPost post);
    }
}