using AutoMapper;
using Financia.Communication.Responses;
using Financia.Domain.Services.LoggedUser;

namespace Financia.Application.UseCases.Users.GetProfile
{
    public class GetProfileUseCase : IGetProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;
        public GetProfileUseCase(
            ILoggedUser loggedUser,
            IMapper mapper)
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
        }
        public async Task<ResponseUserProfileJson> Execute()
        {
            var user = await _loggedUser.Get();
            return _mapper.Map<ResponseUserProfileJson>(user);
        }
    }
}
