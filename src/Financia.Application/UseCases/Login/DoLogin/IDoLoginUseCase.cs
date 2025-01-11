using Financia.Communication.Requests;
using Financia.Communication.Responses;

namespace Financia.Application.UseCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseRegisterUserJson> Execute(RequestLoginJson request);
    }
}
