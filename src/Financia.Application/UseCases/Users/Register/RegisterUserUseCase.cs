using AutoMapper;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Domain.Repositories;
using Financia.Domain.Repositories.User;
using Financia.Domain.Security.Cryptography;
using Financia.Domain.Security.Tokens;
using Financia.Exception;
using Financia.Exception.ExceptionBase.Exceptions;
using FluentValidation.Results;

namespace Financia.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {

        private readonly IMapper _mapper;  
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public RegisterUserUseCase( 
            IMapper mapper, 
            IPasswordEncrypter passwordEncrypter,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUserWriteOnlyRepository userWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IAccessTokenGenerator accessTokenGenerator
            )
        {
            _mapper = mapper;
            _passwordEncrypter = passwordEncrypter;
            _userReadOnlyRepository = userReadOnlyRepository;
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);
            user.Password = _passwordEncrypter.Encrypt(user.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _userWriteOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new ResponseRegisterUserJson
            {
                Name = user.Name,
                Token = _accessTokenGenerator.Generate(user)
            };
        }

        private async Task Validate(RequestRegisterUserJson request) 
        {
            var result = new RegisterUserValidator().Validate(request);

            var existEmail = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (existEmail)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false) 
            { 
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
