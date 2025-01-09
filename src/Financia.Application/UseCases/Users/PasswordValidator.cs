using Financia.Communication.Requests;
using Financia.Exception;
using FluentValidation;
using FluentValidation.Validators;

namespace Financia.Application.UseCases.Users
{
    public class PasswordValidator<T> : PropertyValidator<T , string>
    {
        public override string Name => "PasswordValidator";

        public override bool IsValid(ValidationContext<T> context, string password)
        {
            
        }
    }
}
