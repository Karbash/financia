namespace Financia.Domain.Security.Tokens
{
    public interface ITokenProvider
    {
        string TokenOnRequest();
    }
}
