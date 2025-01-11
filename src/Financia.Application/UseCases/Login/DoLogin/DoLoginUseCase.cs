using Financia.Application.UseCases.Expenses;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Domain.Repositories.User;
using Financia.Domain.Security.Cryptography;
using Financia.Domain.Security.Tokens;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase: IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public DoLoginUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IPasswordEncrypter passwordEncrypter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordEncrypter = passwordEncrypter; 
        }  
        
        public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
        {
            Validate(request);

            var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);

            if (user is null)
            {
                throw new InvalidLoginException();
            }

            var passwordMatch = _passwordEncrypter.Verify(request.Password, user.Password);

            if (passwordMatch == false)
            {
                throw new InvalidLoginException();
            }

            return new ResponseRegisterUserJson
            {
                Name = user.Name,
                Token = _accessTokenGenerator.Generate(user)
            };
        }

        private void Validate(RequestLoginJson request)
        {

            var validator = new LoginValidator();
            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors
                                    .Select(f => f.ErrorMessage)
                                    .ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
