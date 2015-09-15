namespace SlackClient
{
    public interface ISlackLogger
    {
        SlackLogResult Log(SlackPost post);
    }
}