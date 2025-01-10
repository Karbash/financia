using AutoMapper;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Domain.Repositories.User;
using Financia.Domain.Security.Cryptography;
using Financia.Exception;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {

        private readonly IMapper _mapper;  
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        
        public RegisterUserUseCase( 
            IMapper mapper, 
            IPasswordEncrypter passwordEncrypter,
            IUserReadOnlyRepository userReadOnlyRepository
            )
        {
            _mapper = mapper;
            _passwordEncrypter = passwordEncrypter;
            _userReadOnlyRepository = userReadOnlyRepository;
        }
        public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _passwordEncrypter.Encrypt(user.Password);

            return new ResponseRegisterUserJson
            {
                Name = user.Name,
                Token = ""
            };
        }

        private async Task Validate(RequestRegisterUserJson request) 
        {
            var result = new RegisterUserValidator().Validate(request);

            var existEmail = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (existEmail)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false) 
            { 
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
