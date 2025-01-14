using Financia.Domain.Entities;

namespace Financia.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        Task<User> Get();
    }
}
