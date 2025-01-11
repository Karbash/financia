using Financia.Domain.Entities;

namespace Financia.Domain.Security.Tokens
{
    public interface IAccessTokenGenerator
    {
        string Generate(User user);
    }
}
