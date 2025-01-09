using AutoMapper;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {

        private readonly IMapper _mapper;  
        
        public RegisterUserUseCase(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
        {
            Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            return new ResponseRegisterUserJson
            {
                Name = user.Name,
                Token = ""
            };
        }

        private void Validate(RequestRegisterUserJson request) 
        {
            var result = new RegisterUserValidator().Validate(request);
            if (result.IsValid == false) 
            { 
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
