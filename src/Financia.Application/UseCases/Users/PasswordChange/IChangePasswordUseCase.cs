using Financia.Communication.Requests;

namespace Financia.Application.UseCases.Users.PasswordChange
{
    public interface IChangePasswordUseCase
    {
        Task Execute(RequestChangePasswordJson request);
    }
}
