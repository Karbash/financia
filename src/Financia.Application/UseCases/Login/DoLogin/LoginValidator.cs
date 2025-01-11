using Financia.Application.UseCases.Users;
using Financia.Communication.Requests;
using Financia.Exception;
using FluentValidation;

namespace Financia.Application.UseCases.Login.DoLogin
{
    public class LoginValidator : AbstractValidator<RequestLoginJson>
    {
        public LoginValidator()
        {
            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestLoginJson>());
            RuleFor(login => login.Email)
                .NotEmpty()
                .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
                .EmailAddress();
        }
    }
}
