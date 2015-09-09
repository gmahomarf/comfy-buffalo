namespace Ironhide.Users.Domain.Services
{
    public interface IIdentityGenerator<out T>
    {
        T Generate();
    }
}