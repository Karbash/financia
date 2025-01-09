using Financia.Communication.Requests;
using Financia.Communication.Responses;

namespace Financia.Application.UseCases.Users.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
    }
}
