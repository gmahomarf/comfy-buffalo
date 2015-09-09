namespace Starscream.Domain
{
    public interface ITokenGenerator<out T>
    {
        T Generate();
    }
}