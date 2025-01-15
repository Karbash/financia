using Financia.Communication.Requests;
using Financia.Domain.Entities;
using Financia.Domain.Repositories;
using Financia.Domain.Repositories.User;
using Financia.Domain.Security.Cryptography;
using Financia.Domain.Services.LoggedUser;
using Financia.Exception;
using Financia.Exception.ExceptionBase.Exceptions;
using FluentValidation.Results;

namespace Financia.Application.UseCases.Users.PasswordChange
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordUseCase(
                    ILoggedUser loggedUser,
                    IPasswordEncrypter passwordEncrypter,
                    IUserUpdateOnlyRepository userUpdateOnlyRepository,
                    IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _passwordEncrypter = passwordEncrypter;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.Get();

            Validate(request, loggedUser);

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);
            user.Password = _passwordEncrypter.Encrypt(request.NewPassword);

            _userUpdateOnlyRepository.Update(user);
            await _unitOfWork.Commit();

        }

        private void Validate(RequestChangePasswordJson request, User loggedUser)
        {
            var validator = new ChangePasswordValidator();
            var result = validator.Validate(request);

            var passwordMatch = _passwordEncrypter.Verify(request.Password, loggedUser.Password);

            if(!passwordMatch)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
            }
            if(!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
