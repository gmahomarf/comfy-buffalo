using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain.Services
{
    public interface ITokenFactory
    {
        string Create(User executor);
    }
}