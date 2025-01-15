using Financia.Communication.Requests;

namespace Financia.Application.UseCases.Users.UpdateProfile
{
    public interface IUpdateUserUseCase
    {
        Task Execute(RequestUpdateUserJson request);
    }
}
