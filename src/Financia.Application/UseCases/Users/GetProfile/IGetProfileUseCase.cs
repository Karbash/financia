using Financia.Communication.Responses;

namespace Financia.Application.UseCases.Users.GetProfile
{
    public interface IGetProfileUseCase
    {
        Task<ResponseUserProfileJson> Execute();
    }
}
