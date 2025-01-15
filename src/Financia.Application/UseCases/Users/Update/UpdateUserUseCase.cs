using Financia.Application.UseCases.Users.Update;
using Financia.Communication.Requests;
using Financia.Domain.Repositories;
using Financia.Domain.Repositories.User;
using Financia.Domain.Services.LoggedUser;
using Financia.Exception;
using Financia.Exception.ExceptionBase.Exceptions;
using FluentValidation.Results;

namespace Financia.Application.UseCases.Users.UpdateProfile
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _updateRepository;
        private readonly IUserReadOnlyRepository _readRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(ILoggedUser loggedUser,
                                 IUserUpdateOnlyRepository updateRepository,
                                 IUserReadOnlyRepository readRepository,
                                 IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _updateRepository = updateRepository;
            _readRepository = readRepository;
            _unitOfWork = unitOfWork;
        }                       

        public async Task Execute(RequestUpdateUserJson request)
        {
            var loggedUser = await _loggedUser.Get();

            await Validate(request, loggedUser.Email);

            var user = await _updateRepository.GetById(loggedUser.Id);

            user.Name= request.Name;
            user.Email = request.Email;

            _updateRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateUserJson request, string currentEmail)
        {
            var validator = new UpdateUserValidator();

            var result = validator.Validate(request);

            if (currentEmail.Equals(request.Email) == false)
            {
                var userExist = await _readRepository.ExistActiveUserWithEmail(request.Email);
                if (userExist)
                {
                    result.Errors.Add(new ValidationFailure(string.Empty,ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
                }
            }

            if(result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
