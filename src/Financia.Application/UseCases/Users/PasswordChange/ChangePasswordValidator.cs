using Financia.Communication.Requests;
using FluentValidation;

namespace Financia.Application.UseCases.Users.PasswordChange
{
    public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
    {
        public ChangePasswordValidator() 
        {
            RuleFor(user => user.Password)
                .SetValidator(new PasswordValidator<RequestChangePasswordJson>());
        }
    }
}
